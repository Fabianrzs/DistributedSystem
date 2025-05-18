using Microsoft.EntityFrameworkCore;

namespace TryAdminBack.Infrastructure.Implementations.Persistence.Seeders;

public interface ISeeder<in TDbContext> where TDbContext : DbContext
{
    Task Seed(TDbContext context);
}
