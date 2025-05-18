using WebApi.Settings;

namespace WebApi.Extensions;

internal static class CorsExtensions
{
    private const string CorsPolicyName = "CustomCorsPolicy";

    public static IServiceCollection AddCorsConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        CorsSettings? corsSettings = configuration.GetSection("CorsSettings").Get<CorsSettings>();

        if (corsSettings != null && corsSettings.EnableCors)
        {
            services.AddCors(options => options.AddPolicy(CorsPolicyName, builder =>
            {
                if (corsSettings.AllowAllOrigins)
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                }
                else
                {
                    builder.WithOrigins(corsSettings.AllowedOrigins ?? [])
                           .WithMethods(corsSettings.AllowedMethods ?? [])
                           .WithHeaders(corsSettings.AllowedHeaders ?? []);

                    if (corsSettings.AllowCredentials)
                    {
                        builder.AllowCredentials();
                    }
                }
            }));
        }

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
