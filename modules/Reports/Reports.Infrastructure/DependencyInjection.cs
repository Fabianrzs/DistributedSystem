using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Reports.Infrastructure.Extensions;

namespace Reports.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureReport(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddPersistenceReport(configuration);
        services.AddRepositoriesReport();
        services.AddServicesReport(configuration);
        return services;
    }
}

