using Microsoft.Extensions.DependencyInjection;
using Notifications.Domain.Modules.Repositories;
using Notifications.Infrastructure.Implementations.Repository;

namespace Notifications.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureNotifi(this IServiceCollection services)
    {
        services.AddScoped<IEmailRepository, EmailRepository>();
        return services;
    }
}
