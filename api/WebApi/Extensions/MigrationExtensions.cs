using Authentications.Infrastructure.Implementations.Persistence.EFCore;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Extensions;

public static class MigrationExtensions
{
    public static void ApplyMigrations(this IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();
        using AuthenticationDbContext dbContextAuthentication =
            scope.ServiceProvider.GetRequiredService<AuthenticationDbContext>();
        dbContextAuthentication.Database.Migrate();
    }

}
