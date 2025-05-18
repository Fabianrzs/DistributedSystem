using Application.Abstractions.Security;
using Infrastructure.Implementations.Security;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Extensions;

public static class SecurityExtension
{
    public static IServiceCollection AddAuthenticationUser(this IServiceCollection services)
    {
        services.AddScoped<IUserContext, UserContext>();
        return services;
    }
}
