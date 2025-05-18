using Authentications.Domain.Modules.Entities;
using Template.Domain.Abstractions.Repositories;

namespace Authentications.Domain.Modules.Repositories;
public interface IAuthenticationRepository : IRepository<User>
{
    Task<User?> SignInAsync(string email, string password);
    Task SignOutAsync(Guid sessionId);
    Task<bool> CheckSessionAsync(Guid userId, Guid sessionId);
    Task<User> SignUpAsync(User user);
    Task<Session> CreateSessionAsync(Guid userId);
    Task<Session?> GetActiveSessionAsync(Guid userId);
}
