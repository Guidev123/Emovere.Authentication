using Authentication.API.Configurations;

var builder =
    WebApplication.CreateBuilder(args)
    .AddUseCases()
    .AddCustomMiddlewares()
    .AddMessageBusConfiguration()
    .AddgRPCServices()
    .AddDbContextConfig()
    .AddEmailServicesConfiguration()
    .AddIdentityConfig()
    .AddSwaggerConfig()
    .AddSerilog()
    .AddModelsSettings()
    .AddApplicationServices()
    .AddInfrastructureServices()
    .AddServices();

var app = builder.Build()
    .UseApiDefaultSeetings()
    .UseSerilogSettings();

app.Run();