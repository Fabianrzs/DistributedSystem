using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Implementations.Persistence.Seeders;

public interface ISeeder<in TDbContext> where TDbContext : DbContext
{
    Task Seed(TDbContext context);
}
