using Authentication.API.Application.Commands.Users.Delete;
using Authentication.API.Driven.Ports.Services;
using Authentication.API.Extensions;
using Emovere.SharedKernel.Abstractions;
using Emovere.SharedKernel.Notifications;
using Emovere.SharedKernel.Responses;

namespace Authentication.API.Application.Commands.Users.Register
{
    public sealed class RegisterUserHandler(INotificator notificator,
                                            IAspNetIdentityService identityService,
                                            ICustomerGrpcService customerGrpc)
                                          : CommandHandler<RegisterUserCommand, RegisterUserResponse>(notificator)
    {
        public override async Task<Response<RegisterUserResponse>> ExecuteAsync(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            if (!ExecuteValidation(new RegisterUserValidator(), request))
                return Response<RegisterUserResponse>.Failure(Notifications, ResponseMessages.INVALID_USER_CREDENTIALS);

            var identityResult = await identityService.RegisterAsync(request).ConfigureAwait(false);
            if (!identityResult.IsSuccess || identityResult.Data is null) return identityResult;

            var createCustomerResult = await customerGrpc.CreateAsync(request, identityResult.Data.Id).ConfigureAwait(false);
            if (!createCustomerResult.IsSuccess)
            {
                await identityService.DeleteAsync(new DeleteUserCommand(identityResult.Data.Id)).ConfigureAwait(false);

                Notify(ResponseMessages.FAILED_TO_REGISTER_CUSTOMER);
                return Response<RegisterUserResponse>.Failure(Notifications, createCustomerResult.Message);
            }

            return identityResult;
        }
    }
}