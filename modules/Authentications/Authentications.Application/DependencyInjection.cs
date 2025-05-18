using Application.Abstractions.Behaviors;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Authentications.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationAuth(this IServiceCollection services)
    {

        TypeAdapterConfig config = TypeAdapterConfig.GlobalSettings;

        config.Scan(typeof(DependencyInjection).Assembly);

        services.AddSingleton(config);

        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
            config.AddOpenBehavior(typeof(RequestLoggingPipelineBehavior<,>));
            config.AddOpenBehavior(typeof(ValidationPipelineBehavior<,>));
        });

        services.AddSingleton<INotificationPublisher, DomainNotificationLoggingPublisher>();

        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly, includeInternalTypes: true);

        return services;
    }
}
