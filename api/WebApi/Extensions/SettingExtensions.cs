using Notifications.Infrastructure.Settings;

namespace WebApi.Extensions;

public static class SettingExtensions
{
    public static IServiceCollection AddSettingExtensions(this IServiceCollection services, IConfiguration configuration)
    {

        IConfigurationSection mongoConfig = configuration.GetSection("MongoDbSettings")
            ?? throw new InvalidOperationException("MongoDbSettings section is missing in configuration.");

        services.Configure<MongoDbSettings>(mongoConfig);

        IConfigurationSection emailConfig = configuration.GetSection("EmailSettings")
            ?? throw new InvalidOperationException("MongoDbSettings section is missing in configuration.");

        services.Configure<EmailSettings>(emailConfig);

        return services;
    }
}
