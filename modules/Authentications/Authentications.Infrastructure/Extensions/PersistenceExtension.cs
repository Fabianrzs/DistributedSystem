using Authentications.Infrastructure.Implementations.Persistence.EFCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Authentications.Infrastructure.Extensions;

public static class PersistenceExtension
{
    public static IServiceCollection AddPersistenceAuth(this IServiceCollection services, IConfiguration configuration)
    {

        string authConnection = configuration.GetConnectionString("AuthenticationConnection");

        services.AddDbContextFactory<AuthenticationDbContext>(options =>
        {
            options.UseSqlServer(authConnection, sqlOptions =>
            {
                sqlOptions.MigrationsHistoryTable("__EFMigrationsHistory_AuthDb");
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
