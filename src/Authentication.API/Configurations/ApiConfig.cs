using Authentication.API.Driven.Adapters.Data;
using Authentication.API.Driven.Adapters.Services;
using Authentication.API.Driven.Ports.Services;
using Authentication.API.Driving.Adapters.Endpoints;
using Authentication.API.Driving.Ports.Services;
using Authentication.API.Extensions;
using Authentication.API.Middlewares;
using Emovere.Infrastructure.Bus;
using Emovere.Infrastructure.Email;
using Emovere.Infrastructure.EventSourcing;
using Emovere.SharedKernel.Abstractions.Mediator;
using Emovere.SharedKernel.Notifications;
using KeyPairJWT.AspNet;
using KeyPairJWT.Configuration;
using KeyPairJWT.Core;
using KeyPairJWT.Core.Jwa;
using KeyPairJWT.EntityFramework;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MidR.DependencyInjection;
using SendGrid.Extensions.DependencyInjection;
using Serilog;
using System.Reflection;

namespace Authentication.API.Configurations
{
    public static class ApiConfig
    {
        public static WebApplicationBuilder AddUseCases(this WebApplicationBuilder builder)
        {
            builder.Services.AddMidR(Assembly.GetExecutingAssembly());
            builder.Services.AddScoped<IMediatorHandler, MediatorHandler>();
            builder.Services.AddScoped<INotificator, Notificator>();

            return builder;
        }

        public static WebApplicationBuilder AddMessageBusConfiguration(this WebApplicationBuilder builder)
        {
            builder.Services.AddMessageBus(builder.Configuration.GetConnectionString("MessageBusConnection") ?? string.Empty);

            return builder;
        }

        public static WebApplicationBuilder AddDbContextConfig(this WebApplicationBuilder builder)
        {
            builder.Services.AddDbContext<AuthenticationDbContext>(opt =>
                    opt.UseSqlServer(builder.Configuration.GetConnectionString("DatabaseConnection")));

            return builder;
        }

        public static WebApplicationBuilder AddInfrastructureServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IAspNetIdentityService, AspNetIdentityService>();
            builder.Services.AddScoped<ITokenService, TokenService>();

            return builder;
        }

        public static WebApplicationBuilder AddEmailServicesConfiguration(this WebApplicationBuilder builder)
        {
            builder.Services.AddSendGrid(x =>
            {
                x.ApiKey = builder.Configuration.GetValue<string>("EmailSettings:ApiKey");
            });
            builder.Services.AddScoped<IEmailService, EmailService>();

            return builder;
        }

        public static WebApplicationBuilder AddServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddOpenApi();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddEventStoreConfiguration();

            return builder;
        }

        public static WebApplicationBuilder AddIdentityConfig(this WebApplicationBuilder builder)
        {
            builder.Services.AddDefaultIdentity<IdentityUser>()
                    .AddRoles<IdentityRole>()
                    .AddEntityFrameworkStores<AuthenticationDbContext>()
                    .AddDefaultTokenProviders();

            builder.Services.Configure<DataProtectionTokenProviderOptions>(options =>
            {
                options.TokenLifespan = TimeSpan.FromHours(2);
            });

            builder.Services.AddJwtConfiguration(builder.Configuration);
            builder.Services.AddAuthorization();

            builder.Services.AddJwksManager(x => x.Jws = Algorithm.Create(DigitalSignaturesAlgorithm.EcdsaSha256))
                .PersistKeysToDatabaseStore<AuthenticationDbContext>()
                .UseJwtValidation();

            return builder;
        }

        public static WebApplicationBuilder AddModelsSettings(this WebApplicationBuilder builder)
        {
            builder.Services.Configure<AppTokenSettings>(builder.Configuration.GetSection(nameof(AppTokenSettings)));

            return builder;
        }

        public static WebApplicationBuilder AddCustomMiddlewares(this WebApplicationBuilder builder)
        {
            builder.Services.AddTransient<GlobalExceptionMiddleware>();

            return builder;
        }

        public static WebApplicationBuilder AddSerilog(this WebApplicationBuilder builder)
        {
            builder.Host.UseSerilog((context, services, configuration) =>
            {
                configuration.ReadFrom.Configuration(context.Configuration);

                if (context.HostingEnvironment.IsDevelopment())
                    configuration.WriteTo.Debug();
            });

            return builder;
        }

        public static WebApplicationBuilder AddApplicationServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddTransient<IUserService, UserService>();

            return builder;
        }

        public static WebApplication UseApiDefaultSeetings(this WebApplication app)
        {
            app.UseMiddleware<GlobalExceptionMiddleware>();

            app.UseSwaggerConfig();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthConfiguration();

            app.MapEndpoints();

            app.UseJwksDiscovery();

            return app;
        }

        public static WebApplication UseSerilogSettings(this WebApplication app)
        {
            app.UseSerilogRequestLogging(options =>
            {
                options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
                {
                    diagnosticContext.Set("RequestHost", httpContext.Request.Host);
                    diagnosticContext.Set("RequestPath", httpContext.Request.Path);
                    diagnosticContext.Set("RequestMethod", httpContext.Request.Method);
                };
            });

            return app;
        }
    }
}