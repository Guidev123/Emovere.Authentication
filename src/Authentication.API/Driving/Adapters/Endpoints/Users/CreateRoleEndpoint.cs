using Authentication.API.Application.Commands.Users.CreateRole;
using Authentication.API.Driving.Ports.Services;

namespace Authentication.API.Driving.Adapters.Endpoints.Users
{
    public sealed class CreateRoleEndpoint : IEndpoint
    {
        public static void Map(IEndpointRouteBuilder app)
            => app.MapPost("/roles", HandleAsync).RequireAuthorization();

        private static async Task<IResult> HandleAsync(CreateRoleCommand command, IUserService userService)
            => Endpoint.CustomResponse(await userService.CreateRoleAsync(command).ConfigureAwait(false));
    }
}