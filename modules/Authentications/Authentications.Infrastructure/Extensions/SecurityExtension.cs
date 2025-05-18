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
        string accessSecret = configuration["Jwt:AccessSecret"]!
            ?? throw new ArgumentException("AccessSecret is not configured.");

        string refreshSecret = configuration["Jwt:RefreshSecret"]!
            ?? throw new ArgumentException("RefreshSecret is not configured.");

        string issuer = configuration["Jwt:Issuer"]
            ?? throw new ArgumentException("Issuer is not configured.");

        string audience = configuration["Jwt:Audience"]
            ?? throw new ArgumentException("Audience is not configured.");
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
           .AddJwtBearer(o =>
           {
               o.RequireHttpsMetadata = false;
               o.TokenValidationParameters = new TokenValidationParameters
               {
                   IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(accessSecret)),
                   ValidIssuer = issuer,
                   ValidAudience = audience,
                   ClockSkew = TimeSpan.Zero
               };
           }).AddJwtBearer("Refresh", options =>
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
