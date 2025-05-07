using Authentication.API.Application.Commands.Users.ForgetPassword;
using Authentication.API.Driving.Ports.Services;

namespace Authentication.API.Driving.Adapters.Endpoints.Users
{
    public sealed class ForgetPasswordEndpoint : IEndpoint
    {
        public static void Map(IEndpointRouteBuilder app)
            => app.MapPost("/password/forget", HandleAsync);

        private static async Task<IResult> HandleAsync(ForgetUserPasswordCommand command, IUserService userService)
            => Endpoint.CustomResponse(await userService.ForgetPasswordAsync(command).ConfigureAwait(false));
    }
}