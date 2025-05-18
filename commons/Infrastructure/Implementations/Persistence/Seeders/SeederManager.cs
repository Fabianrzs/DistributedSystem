using Infrastructure.Implementations.Persistence.EFCore.Contexts;
using TryAdminBack.Infrastructure.Implementations.Persistence.Seeders;

namespace Infrastructure.Implementations.Persistence.Seeders;

public class SeederManager(List<ISeeder<ApplicationsDbContext>> seeders)
{
    public async Task SeedDatabase(ApplicationsDbContext context)
    {
        foreach (ISeeder<ApplicationsDbContext> seeder in seeders)
        {
            await seeder.Seed(context);
            await context.SaveChangesAsync();
        }
    }
}
