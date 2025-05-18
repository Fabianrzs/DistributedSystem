using System.Reflection;
using WebApi.Extensions;
using WebApi.Infrastructure;

namespace WebApi;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services, IConfiguration configuration)
    {
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        services.AddOpenApi();
        services.AddSwaggerGenWithAuth();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddHealthChecks();
        services.AddControllers();
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();
        services.AddEndpoints(Assembly.GetExecutingAssembly());
        services.AddCorsConfiguration(configuration);
        services.AddHttpContextAccessor();

        return services;
    }

    public static WebApplication UsePresentation(this WebApplication app, IConfiguration configuration)
    {
        app.MapEndpoints();

        app.UseCorsConfiguration(configuration);

        if (!app.Environment.IsProduction())
        {
            app.MapOpenApi();
            app.UseSwaggerWithUi();
        }

        app.UseRequestContextLogging();
        app.UseHttpsRedirection();
        app.UseExceptionHandler();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();
        return app;
    }
}
