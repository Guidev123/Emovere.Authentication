using Authentication.API.Application.Commands.Users.Register;
using Authentication.API.Driven.Ports.Services;
using Customers.Protos;
using Emovere.SharedKernel.Notifications;
using Emovere.SharedKernel.Responses;
using Google.Protobuf.WellKnownTypes;

namespace Authentication.API.Driven.Adapters.Services
{
    public class CustomerGrpcService(CustomerEndpoint.CustomerEndpointClient customerEndpoint,
                                            INotificator notificator) : ICustomerGrpcService
    {
        public async Task<Response> CreateAsync(RegisterUserCommand command, Guid userId)
        {
            var grpcResponse = await customerEndpoint.CreateCustomerAsyncAsync(new CreateCustomerRequest
            {
                UserId = userId.ToString(),
                FirstName = command.FirstName,
                LastName = command.LastName,
                Email = command.Email,
                Document = command.Document,
                BirthDate = Timestamp.FromDateTime(command.BirthDate.ToUniversalTime())
            });

            if (!grpcResponse.IsSuccess)
                return Response.Failure([.. notificator.GetNotifications().Select(x => x.Message)]);

            return Response.Success(StatusCode.CREATED_STATUS_CODE);
        }
    }
}