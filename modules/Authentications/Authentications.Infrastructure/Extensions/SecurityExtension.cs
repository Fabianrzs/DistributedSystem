using System.Text;
using Authentications.Application.Abstractions.Security;
using Authentications.Infrastructure.Implementations.Security.Authentication;
using Authentications.Infrastructure.Implementations.Security.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using TryAdminBack.Infrastructure.Implementations.Security.Authorization;

namespace Authentications.Infrastructure.Extensions;

public static class SecurityExtension
{
    public static IServiceCollection AddAuthenticationInternal(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        string accessSecret = configuration["SECURITY-JWT-SECRET-ACCESS"]
            ?? throw new ArgumentException("SECURITY-JWT-SECRET-ACCESS is not configured.");

        string refreshSecret = configuration["SECURITY-JWT-SECRET-REFRESH"]
            ?? throw new ArgumentException("SECURITY-JWT-SECRET-REFRESH is not configured.");

        string issuer = configuration["SECURITY-JWT-ISSUER"]
            ?? throw new ArgumentException("SECURITY-JWT-ISSUER is not configured.");

        string audience = configuration["SECURITY-JWT-AUDIENCE"]
            ?? throw new ArgumentException("SECURITY-JWT-AUDIENCE is not configured.");

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(accessSecret)),
                    ValidIssuer = issuer,
                    ValidAudience = audience,
                    ClockSkew = TimeSpan.Zero
                };
            })
            .AddJwtBearer("Refresh", options =>
            {
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(refreshSecret)),
                    ValidIssuer = issuer,
                    ValidAudience = audience,
                    ClockSkew = TimeSpan.Zero
                };
            });

        services.AddHttpContextAccessor();
        services.AddScoped<IUserContext, UserContext>();
        services.AddSingleton<ITokenProvider, TokenProvider>();

        return services;
    }

    public static IServiceCollection AddAuthorizationInternal(this IServiceCollection services)
    {
        services.AddAuthorization();

        services.AddScoped<PermissionProvider>();
        services.AddTransient<IAuthorizationHandler, PermissionAuthorizationHandler>();
        services.AddTransient<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();

        return services;
    }
}
