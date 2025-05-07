using Authentication.API.Application.Commands.Users.Login;
using Authentication.API.Driving.Ports.Services;

namespace Authentication.API.Driving.Adapters.Endpoints.Users
{
    public sealed class LoginEndpoint : IEndpoint
    {
        public static void Map(IEndpointRouteBuilder app)
            => app.MapPost("/login", HandleAsync);

        private static async Task<IResult> HandleAsync(LoginUserCommand command, IUserService userService)
            => Endpoint.CustomResponse(await userService.LoginAsync(command).ConfigureAwait(false));
    }
}