using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Reports.Application.Abstractions.FileProcessor;
using Reports.Infrastructure.Implementations.FileProcessor;

namespace Reports.Infrastructure.Extensions;
public static class ServicesExtension
{
    public static IServiceCollection AddServicesReport(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<IFileProcessingService, FileProcessingService>();
        return services;
    }
}
