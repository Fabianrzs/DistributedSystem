using Authentications.Infrastructure.Extensions;
using Infrastructure.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Authentications.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureAuth(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddPersistenceAuth(configuration);
        services.AddRepositories(configuration);
        services.AddServices(configuration);
        services.AddAuthenticationInternal(configuration);
        services.AddAuthorizationInternal();
        return services;
    }
}
