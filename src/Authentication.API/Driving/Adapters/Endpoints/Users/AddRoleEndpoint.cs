using Authentication.API.Application.Commands.Users.AddRole;
using Authentication.API.Driving.Ports.Services;

namespace Authentication.API.Driving.Adapters.Endpoints.Users
{
    public sealed class AddRoleEndpoint : IEndpoint
    {
        public static void Map(IEndpointRouteBuilder app)
            => app.MapPost("/roles/users", HandleAsync);

        private static async Task<IResult> HandleAsync(AddUserRoleCommand command, IUserService userService)
            => Endpoint.CustomResponse(await userService.AddRoleAsync(command).ConfigureAwait(false));
    }
}