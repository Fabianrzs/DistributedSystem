using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Reports.Application.Abstractions.Cache;
using Reports.Infrastructure.Extensions;
using Reports.Infrastructure.Implementations.Cache;
using StackExchange.Redis;

namespace Reports.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureReport(this IServiceCollection services, IConfiguration configuration)
    {
        string redisConnectionString = configuration.GetConnectionString("RedisConnection");

        services.AddSingleton<IConnectionMultiplexer>(sp =>
        {
            var configuration = ConfigurationOptions.Parse(redisConnectionString!);
            configuration.AbortOnConnectFail = false;
            return ConnectionMultiplexer.Connect(configuration);
        });
        services.AddSingleton<IDistributedCacheService, DistributedCacheService>();
        services.AddPersistenceReport(configuration);
        services.AddRepositoriesReport();
        services.AddServicesReport(configuration);
        return services;
    }
}

