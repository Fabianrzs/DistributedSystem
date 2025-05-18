using Application;
using Authentications.Application;
using Authentications.Infrastructure;
using Infrastructure;
using WebApi;


WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

IServiceCollection services = builder.Services;
IConfigurationManager configuration = builder.Configuration;

services
    .AddInfrastructure(configuration)
    .AddInfrastructureAuth(configuration);

services.AddApplication()
    .AddApplicationAuth();

services.AddPresentation(configuration);


WebApplication app = builder.Build();

app.UsePresentation(configuration);

await app.RunAsync();
public partial class Program;
