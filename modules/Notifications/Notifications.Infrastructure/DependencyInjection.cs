using Microsoft.Extensions.DependencyInjection;
using Notifications.Application.Abstractions.Emails;
using Notifications.Domain.Modules.Repositories;
using Notifications.Infrastructure.Implementations.Emails;
using Notifications.Infrastructure.Implementations.Repository;

namespace Notifications.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureNotifi(this IServiceCollection services)
    {
        services.AddScoped<IEmailRepository, EmailRepository>();
        services.AddScoped<IEmailService, EmailService>();
        return services;
    }
}
