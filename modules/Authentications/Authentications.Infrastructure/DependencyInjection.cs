using Authentications.Infrastructure.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Authentications.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureAuth(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddPersistenceAuth(configuration);
        services.AddRepositoriesAuth();
        services.AddServicesAuth(configuration);
        services.AddAuthenticationInternal(configuration);
        services.AddAuthorizationInternal();
        return services;
    }
}
