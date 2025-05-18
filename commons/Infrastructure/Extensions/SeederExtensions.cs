using Infrastructure.Implementations.Persistence.EFCore.Contexts;
using Infrastructure.Implementations.Persistence.Seeders;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using TryAdminBack.Infrastructure.Implementations.Persistence.Seeders;

namespace Infrastructure.Extensions;

public static class SeederExtensions
{
    public static async Task RunSeedersAsync<TApplicationsDbContext>(this IServiceProvider serviceProvider, List<ISeeder<TApplicationsDbContext>> seeders) 
        where TApplicationsDbContext : ApplicationsDbContext
    {
        using IServiceScope scope = serviceProvider.CreateScope();
        TApplicationsDbContext context = scope.ServiceProvider.GetRequiredService<TApplicationsDbContext>();

        await using IDbContextTransaction transaction = await context.Database.BeginTransactionAsync();
        try
        {
            var seederManager = new SeederManager<TApplicationsDbContext>(seeders);
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
