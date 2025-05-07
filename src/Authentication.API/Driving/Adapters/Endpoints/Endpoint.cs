using Authentication.API.Driving.Adapters.Endpoints.Users;
using Emovere.SharedKernel.Responses;

namespace Authentication.API.Driving.Adapters.Endpoints
{
    public static class Endpoint
    {
        public static void MapEndpoints(this WebApplication app)
        {
            var endpoints = app.MapGroup("");

            endpoints.MapGroup("api/v1/authentication")
                .WithTags("Authentication")
                .MapEndpoint<RegisterEndpoint>()
                .MapEndpoint<LoginEndpoint>()
                .MapEndpoint<DeleteUserEndpoint>()
                .MapEndpoint<CreateRoleEndpoint>()
                .MapEndpoint<AddRoleEndpoint>()
                .MapEndpoint<ResetPasswordEndpoint>()
                .MapEndpoint<ForgetPasswordEndpoint>();
        }

        public static IResult CustomResponse<T>(Response<T> response)
        {
            return response.Code switch
            {
                StatusCode.OK_STATUS_CODE => Results.Ok(response),
                StatusCode.CREATED_STATUS_CODE => Results.Created(string.Empty, response),
                StatusCode.NO_CONTENT_STATUS_CODE => Results.NoContent(),
                StatusCode.BAD_REQUEST_STATUS_CODE => Results.BadRequest(response),
                StatusCode.NOT_FOUND_STATUS_CODE => Results.NotFound(response),
                _ => Results.InternalServerError(response)
            };
        }

        private static IEndpointRouteBuilder MapEndpoint<TEndpoint>(this IEndpointRouteBuilder app)
            where TEndpoint : IEndpoint
        {
            TEndpoint.Map(app);
            return app;
        }
    }
}