using Authentications.Application.Abstractions.Security;
using Authentications.Domain.Modules.Entities;
using Authentications.Domain.Modules.Repositories;
using Authentications.Infrastructure.Implementations.Persistence.EFCore;
using Infrastructure.Implementations.Persistence.EFCore.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Authentications.Infrastructure.Implementations.Repositories;

public class AuthenticationRepository(AuthenticationDbContext Context, IPasswordHasher passwordHasher)
    : Repository<User>(Context), IAuthenticationRepository
{
    public async Task<User?> SignInAsync(string email, string password)
    {
        User user = await Context.Set<User>()
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.Email == email);

        bool valid = passwordHasher.VerifyPassword(user!.Password, password);

        if (user is null || !valid)
        { return null; }

        return user;
    }

    public async Task SignOutAsync(Guid sessionId)
    {
        Session? session = await Context.Set<Session>().FindAsync(sessionId);
        if (session != null && session.IsActive)
        {
            session.IsActive = false;
        }
        Context.Set<Session>().Update(session!);
        await Context.SaveChangesAsync();

    }

    public async Task<bool> CheckSessionAsync(Guid userId, Guid sessionId)
    {
        return await Context.Set<Session>()
            .AnyAsync(s => s.Id == sessionId && s.UserId == userId && s.IsActive);
    }

    public async Task<User> SignUpAsync(User user)
    {
        user.Password = passwordHasher.HashPassword(user.Password);
        await Context.Set<User>().AddAsync(user);
        await Context.SaveChangesAsync();
        return user;
    }

    public async Task<Session?> GetActiveSessionAsync(Guid userId)
    {
        return await Context.Set<Session>()
            .FirstOrDefaultAsync(s => s.UserId == userId && s.IsActive);
    }

    public async Task<Session> CreateSessionAsync(Guid userId)
    {
        List<Session> activeSessions = await Context.Set<Session>().Where(s => s.UserId == userId && s.IsActive).ToListAsync();

        activeSessions.ForEach(Session =>
        {
            Session.InActive();
            Context.Set<Session>().Update(Session);
        });

        var session = new Session() { UserId = userId };
        await Context.Set<Session>().AddAsync(session);
        await Context.SaveChangesAsync();
        return session;
    }
}
