using Infrastructure.Implementations.Persistence.EFCore.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Template.Domain.Abstractions.Repositories;

namespace Infrastructure.Extensions;

public static class RepositoriesExtension
{
    public static IServiceCollection AddRepositories(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

        return services;
    }
}
