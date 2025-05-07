using Authentication.API.Driven.Ports.Services;
using Authentication.API.Extensions;
using Emovere.SharedKernel.Abstractions;
using Emovere.SharedKernel.Notifications;
using Emovere.SharedKernel.Responses;

namespace Authentication.API.Application.Commands.Users.Login
{
    public sealed class LoginUserHandler(INotificator notificator, IAspNetIdentityService identityService) : CommandHandler<LoginUserCommand, LoginUserResponse>(notificator)
    {
        public override async Task<Response<LoginUserResponse>> ExecuteAsync(LoginUserCommand request, CancellationToken cancellationToken)
        {
            if (!ExecuteValidation(new LoginUserValidator(), request))
                return Response<LoginUserResponse>.Failure(Notifications, ResponseMessages.INVALID_USER_CREDENTIALS);

            return await identityService.LoginAsync(request).ConfigureAwait(false);
        }
    }
}