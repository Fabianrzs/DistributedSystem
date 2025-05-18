using Microsoft.Extensions.DependencyInjection;

namespace Notifications.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationNotifi(this IServiceCollection services)
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
