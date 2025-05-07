using Authentication.API.Application.Commands.Users.Register;
using Authentication.API.Driving.Adapters.Endpoints;
using Authentication.API.Driving.Ports.Services;

namespace Authentication.API.Driving.Adapters.Endpoints.Users
{
    public sealed class RegisterEndpoint : IEndpoint
    {
        public static void Map(IEndpointRouteBuilder app)
            => app.MapPost("/", HandleAsync);

        private static async Task<IResult> HandleAsync(RegisterUserCommand command, IUserService userService)
            => Endpoint.CustomResponse(await userService.RegisterAsync(command).ConfigureAwait(false));
    }
}