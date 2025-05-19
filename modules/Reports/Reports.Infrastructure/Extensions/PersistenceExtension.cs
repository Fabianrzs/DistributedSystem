using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Reports.Infrastructure.Implementations.Persistence.EFCore;

namespace Reports.Infrastructure.Extensions;

public static class PersistenceExtension
{
    public static IServiceCollection AddPersistenceReport(this IServiceCollection services, IConfiguration configuration)
    {

        string authConnection = configuration.GetConnectionString("ReportConnection");

        services.AddDbContextFactory<ReportDbContext>(options =>
        {
            options.UseSqlServer(authConnection, sqlOptions =>
            {
                sqlOptions.MigrationsHistoryTable("__EFMigrationsHistory_ReportsDb");
                sqlOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
               
            });
            #if DEBUG
            options.EnableSensitiveDataLogging();
            options.LogTo(_ => { }, Microsoft.Extensions.Logging.LogLevel.None);
            #endif
            options.ConfigureWarnings(warnings =>
                warnings.Throw(RelationalEventId.MultipleCollectionIncludeWarning));
        });

        return services;
    }
}
