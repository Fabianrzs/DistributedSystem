using Application;
using Authentications.Application;
using Authentications.Infrastructure;
using Infrastructure;
using MassTransit;
using Notifications.Application;
using Notifications.Application.IntegrationEvents;
using Notifications.Infrastructure;
using WebApi;


WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

IServiceCollection services = builder.Services;
IConfigurationManager configuration = builder.Configuration;


services.AddMassTransit(x =>
{
    // Registrar los consumidores para los eventos que escucharán
    x.AddConsumer<UserSingUpConsumer>();
    x.AddConsumer<UserSingInConsumer>();

    // Configurar RabbitMQ
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("rabbitmq://localhost");  

        cfg.ReceiveEndpoint("user-signup-queue", e => e.ConfigureConsumer<UserSingUpConsumer>(context));

        cfg.ReceiveEndpoint("user-signin-queue", e => e.ConfigureConsumer<UserSingInConsumer>(context));
    });
});


services
    .AddInfrastructure(configuration)
    .AddInfrastructureAuth(configuration)
    .AddInfrastructureNotifi(configuration);

services.AddApplication()
    .AddApplicationAuth()
    .AddApplicationNotifi();

services.AddPresentation(configuration);


WebApplication app = builder.Build();

app.UsePresentation(configuration);

await app.RunAsync();
public partial class Program;
