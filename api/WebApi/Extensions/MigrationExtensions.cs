using Authentications.Infrastructure.Implementations.Persistence.EFCore;
using Microsoft.EntityFrameworkCore;
using Reports.Infrastructure.Implementations.Persistence.EFCore;

namespace WebApi.Extensions;

public static class MigrationExtensions
{
    public static void ApplyMigrations(this IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();
        using AuthenticationDbContext dbContextAuthentication =
            scope.ServiceProvider.GetRequiredService<AuthenticationDbContext>();
        using ReportDbContext dbContextReport =
            scope.ServiceProvider.GetRequiredService<ReportDbContext>();
        
        dbContextAuthentication.Database.Migrate();
        dbContextReport.Database.Migrate();
    }

}
