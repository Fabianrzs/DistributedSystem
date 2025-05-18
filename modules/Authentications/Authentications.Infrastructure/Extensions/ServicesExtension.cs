using Authentications.Application.Abstractions.Security;
using Authentications.Infrastructure.Implementations.Security.Cryptography;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Authentications.Infrastructure.Extensions;
public static class ServicesExtension
{
    public static IServiceCollection AddServicesAuth(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<IPasswordHasher, PasswordHasher>();
        return services;
    }
}
