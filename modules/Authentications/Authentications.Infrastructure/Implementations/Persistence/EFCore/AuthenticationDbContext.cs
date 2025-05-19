using Authentications.Domain.Modules.Entities;
using Authentications.Infrastructure.Implementations.Persistence.EFCore.Configurations;
using Infrastructure.Implementations.Persistence.EFCore.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Authentications.Infrastructure.Implementations.Persistence.EFCore;

public class AuthenticationDbContext(DbContextOptions<AuthenticationDbContext> options) : ApplicationsDbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<Session> Sessions { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new RoleConfiguration());
        modelBuilder.ApplyConfiguration(new UserRoleConfiguration());
        modelBuilder.ApplyConfiguration(new SessionConfiguration());
    }
}
