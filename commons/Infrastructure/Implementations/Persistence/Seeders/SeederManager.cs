using Infrastructure.Implementations.Persistence.EFCore.Contexts;

namespace Infrastructure.Implementations.Persistence.Seeders;

public class SeederManager<TApplicationsDbContext>(List<ISeeder<TApplicationsDbContext>> seeders) where TApplicationsDbContext : ApplicationsDbContext
{
    public async Task SeedDatabase(TApplicationsDbContext context)
    {
        foreach (ISeeder<TApplicationsDbContext> seeder in seeders)
        {
            await seeder.Seed(context);
            await context.SaveChangesAsync();
        }
    }
}
