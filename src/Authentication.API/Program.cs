using Authentication.API.Configurations;
using Emovere.WebApi.Config;

var builder =
    WebApplication.CreateBuilder(args)
    .AddSharedConfig()
    .AddUseCases()
    .AddgRPCServices()
    .AddDbContextConfig()
    .AddIdentityConfig()
    .AddSwaggerConfig()
    .AddModelsSettings()
    .AddApplicationServices()
    .AddInfrastructureServices()
    .AddServices();

var app = builder.Build()
    .UseMiddlewares()
    .UseApiDefaultSeetings()
    .UseSerilogSettings();

app.Run();