using Infrastructure.Implementations.Persistence.EFCore.Contexts;
using Infrastructure.Implementations.Persistence.Seeders;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using TryAdminBack.Infrastructure.Implementations.Persistence.Seeders;

namespace Infrastructure.Extensions;

public static class SeederExtensions
{
    public static async Task RunSeedersAsync(this IServiceProvider serviceProvider, List<ISeeder<ApplicationsDbContext>> seeders)
    {
        using IServiceScope scope = serviceProvider.CreateScope();
        ApplicationsDbContext context = scope.ServiceProvider.GetRequiredService<ApplicationsDbContext>();

        await using IDbContextTransaction transaction = await context.Database.BeginTransactionAsync();
        try
        {
            var seederManager = new SeederManager(seeders);
            await seederManager.SeedDatabase(context);

            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            Console.WriteLine($"Error ejecutando seeders: {ex.Message}");
            throw;
        }
    }
}
