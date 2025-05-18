using Authentications.Domain.Modules.Repositories;
using Authentications.Infrastructure.Implementations.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Authentications.Infrastructure.Extensions;

public static class RepositoriesExtension
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IAuthenticationRepository, AuthenticationRepository>();
        return services;
    }
}
