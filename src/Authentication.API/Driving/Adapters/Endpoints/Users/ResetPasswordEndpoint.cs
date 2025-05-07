using Authentication.API.Application.Commands.Users.ResetPassword;
using Authentication.API.Driving.Ports.Services;

namespace Authentication.API.Driving.Adapters.Endpoints.Users
{
    public sealed class ResetPasswordEndpoint : IEndpoint
    {
        public static void Map(IEndpointRouteBuilder app)
            => app.MapPost("/password/reset", HandleAsync);

        private static async Task<IResult> HandleAsync(ResetUserPasswordCommand command, IUserService userService)
            => Endpoint.CustomResponse(await userService.ResetPasswordAsync(command).ConfigureAwait(false));
    }
}