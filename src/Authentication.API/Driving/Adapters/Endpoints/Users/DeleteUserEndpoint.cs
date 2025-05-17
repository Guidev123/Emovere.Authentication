using Authentication.API.Application.Commands.Users.Delete;
using Authentication.API.Driving.Ports.Services;

namespace Authentication.API.Driving.Adapters.Endpoints.Users
{
    public sealed class DeleteUserEndpoint : IEndpoint
    {
        public static void Map(IEndpointRouteBuilder app)
            => app.MapDelete("/{id:guid}", HandleAsync).RequireAuthorization();

        private static async Task<IResult> HandleAsync(Guid id, IUserService userService)
            => Endpoint.CustomResponse(await userService.DeleteAsync(new DeleteUserCommand(id)).ConfigureAwait(false));
    }
}