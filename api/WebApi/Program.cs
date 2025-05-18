using Application;
using Infrastructure;
using WebApi;


WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

IServiceCollection services = builder.Services;
IConfigurationManager configuration = builder.Configuration;

services
    .AddInfrastructure(configuration)
    .AddApplication()
    .AddPresentation(configuration);

WebApplication app = builder.Build();

await app.RunAsync();
public partial class Program;
