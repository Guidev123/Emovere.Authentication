using Authentication.API.Application.Commands.Users.Login;
using Emovere.SharedKernel.Abstractions.Mediator;

namespace Authentication.API.Driving.Adapters.Endpoints.Users
{
    public sealed class LoginEndpoint : IEndpoint
    {
        public static void Map(IEndpointRouteBuilder app)
            => app.MapPost("/login", HandleAsync);

        private static async Task<IResult> HandleAsync(LoginUserCommand command, IMediatorHandler userService)
            => Endpoint.CustomResponse(await userService.SendCommand(command).ConfigureAwait(false));
    }
}