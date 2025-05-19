using Microsoft.Extensions.DependencyInjection;
using Reports.Domain.Modules.Repositories;
using Reports.Infrastructure.Implementations.Repositories;

namespace Reports.Infrastructure.Extensions;

public static class RepositoriesExtension
{
    public static IServiceCollection AddRepositoriesReport(this IServiceCollection services)
    {
        services.AddScoped<ISalesRepository, SalesRepository>();
        return services;
    }
}
