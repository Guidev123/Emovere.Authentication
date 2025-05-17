using Authentication.API.Driven.Adapters.Data;
using Authentication.API.Driven.Adapters.Services;
using Authentication.API.Driven.Ports.Services;
using Authentication.API.Driving.Adapters.Endpoints;
using Authentication.API.Driving.Ports.Services;
using Authentication.API.Extensions;
using Emovere.WebApi.Config;
using KeyPairJWT.AspNet;
using KeyPairJWT.Core;
using KeyPairJWT.Core.Jwa;
using KeyPairJWT.Store.EntityFramework;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MidR.DependencyInjection;
using System.Reflection;

namespace Authentication.API.Configurations
{
    public static class ApiConfig
    {
        public static WebApplicationBuilder AddUseCases(this WebApplicationBuilder builder)
        {
            builder.Services.AddMidR(Assembly.GetExecutingAssembly());

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

        public static WebApplicationBuilder AddServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddOpenApi();

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

        public static WebApplicationBuilder AddApplicationServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddTransient<IUserService, UserService>();

            return builder;
        }

        public static WebApplication UseApiDefaultSeetings(this WebApplication app)
        {
            app.UseSwaggerConfig();

            app.UseApiSecurityConfig();

            app.UseRouting();

            app.MapEndpoints();

            app.UseJwksDiscovery();

            return app;
        }
    }
}