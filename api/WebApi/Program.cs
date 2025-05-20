using Application;
using Authentications.Application;
using Authentications.Infrastructure;
using Infrastructure;
using Infrastructure.Extensions;
using Infrastructure.Implementations.Persistence.Seeders;
using MassTransit;
using Notifications.Application;
using Notifications.Application.IntegrationEvents;
using Notifications.Infrastructure;
using Reports.Application;
using Reports.Infrastructure;
using Reports.Infrastructure.Implementations.Persistence.EFCore;
using Reports.Infrastructure.Implementations.Persistence.Seeder;
using WebApi;


WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

IServiceCollection services = builder.Services;
IConfigurationManager configuration = builder.Configuration;

string rabbitMqConnectionString = builder.Configuration.GetValue<string>("ConnectionStrings:RabbitConnection");

services.AddMassTransit(x =>
{
    // Registrar los consumidores para los eventos que escucharán
    x.AddConsumer<UserSignUpConsumer>();
    x.AddConsumer<UserSignInConsumer>();

    // Configurar RabbitMQ
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(rabbitMqConnectionString);  

        cfg.ReceiveEndpoint("user-signup-queue", 
            e => e.ConfigureConsumer<UserSignUpConsumer>(context));

        cfg.ReceiveEndpoint("user-signin-queue", 
            e => e.ConfigureConsumer<UserSignInConsumer>(context));
    });
});

services
    .AddInfrastructure(configuration)
    .AddInfrastructureAuth(configuration)
    .AddInfrastructureReport(configuration)
    .AddInfrastructureNotifi();

services.AddApplication()
    .AddApplicationAuth()
    .AddApplicationReport()
    .AddApplicationNotifi();

services.AddPresentation(configuration);


WebApplication app = builder.Build();

app.UsePresentation(configuration);

using (IServiceScope scope = app.Services.CreateScope())
{
    IServiceProvider serviceProvider = scope.ServiceProvider;

    List<ISeeder<ReportDbContext>> seeders = [
        new SalesReportSeeder()
        ];

    await serviceProvider.RunSeedersAsync(seeders);
}

await app.RunAsync();
public partial class Program;
