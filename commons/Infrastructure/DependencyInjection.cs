using Application.Abstractions.Dispachers;
using Infrastructure.Extensions;
using Infrastructure.Implementations.Dispachers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddRepositories(configuration);
        services.AddAuthenticationUser();
        services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();
        return services;
    }
}
