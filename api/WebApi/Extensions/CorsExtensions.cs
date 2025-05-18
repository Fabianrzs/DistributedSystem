namespace WebApi.Extensions;

internal static class CorsExtensions
{
    private const string CorsPolicyName = "CustomCorsPolicy";

    public static IServiceCollection AddCorsConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        string allowedOriginsValue = configuration["CORS-SETTINGS-ALLOWED-ORIGINS"]
            ?? throw new ArgumentException("CORS-SETTINGS-ALLOWED-ORIGINS is not configured.");

        string allowedMethodsValue = configuration["CORS-SETTINGS-ALLOWED-METHODS"]
            ?? throw new ArgumentException("CORS-SETTINGS-ALLOWED-METHODS is not configured.");

        string allowedHeadersValue = configuration["CORS-SETTINGS-ALLOWED-HEADERS"]
            ?? throw new ArgumentException("CORS-SETTINGS-ALLOWED-HEADERS is not configured.");

        string[] allowedOrigins = allowedOriginsValue.Split(",", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        string[] allowedMethods = allowedMethodsValue.Split(",", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        string[] allowedHeaders = allowedHeadersValue.Split(",", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        services.AddCors(options => options.AddPolicy(CorsPolicyName,
            builder => builder.WithOrigins(allowedOrigins)
                       .WithMethods(allowedMethods)
                       .WithHeaders(allowedHeaders)
                       .AllowCredentials()));

        return services;
    }

    public static IApplicationBuilder UseCorsConfiguration(this IApplicationBuilder app, IConfiguration configuration)
    {
        // Si las claves están definidas, se activa la política
        if (configuration["CORS-SETTINGS-ALLOWED-ORIGINS"] is not null)
        {
            app.UseCors(CorsPolicyName);
        }

        return app;
    }
}
