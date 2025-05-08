using Authentication.API.Extensions;
using Emovere.SharedKernel.Responses;
using System.Text.Json;

namespace Authentication.API.Middlewares
{
    public class GlobalExceptionMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                var problemDetails = new
                {
                    Message = ResponseMessages.INTERNAL_SERVER_ERROR,
                    IsSuccess = false,
                    Errors = new string[] { ex.Message }
                };

                string json = JsonSerializer.Serialize(problemDetails);

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = StatusCode.INTERNAL_SERVER_ERROR_STATUS_CODE;
                await context.Response.WriteAsync(json);
            }
        }
    }
}