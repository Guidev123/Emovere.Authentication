using Authentication.API.Driven.Adapters.Services;
using Authentication.API.Driven.Ports.Services;
using Customers.Protos;

namespace Authentication.API.Configurations
{
    public static class GrpcConfig
    {
        public static WebApplicationBuilder AddgRPCServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<ICustomerGrpcService, CustomerGrpcService>();

            builder.Services.AddGrpcClient<CustomerEndpoint.CustomerEndpointClient>(options =>
            {
                options.Address = new Uri(builder.Configuration["GrpcSettings:CustomerUrl"] ?? string.Empty);
            });

            return builder;
        }
    }
}