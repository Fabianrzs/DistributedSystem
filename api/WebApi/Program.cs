using Application;
using Authentications.Application;
using Authentications.Infrastructure;
using Infrastructure;
using Notifications.Application;
using Notifications.Infrastructure;
using WebApi;


WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

IServiceCollection services = builder.Services;
IConfigurationManager configuration = builder.Configuration;

services
    .AddInfrastructure(configuration)
    .AddInfrastructureAuth(configuration)
    .AddInfrastructureNotifi();

services.AddApplication()
    .AddApplicationAuth()
    .AddApplicationNotifi();

services.AddPresentation(configuration);


WebApplication app = builder.Build();

app.UsePresentation(configuration);

await app.RunAsync();
public partial class Program;
