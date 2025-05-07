using Authentication.API.Driven.Ports.Services;
using Emovere.SharedKernel.Abstractions;
using Emovere.SharedKernel.Notifications;
using Emovere.SharedKernel.Responses;

namespace Authentication.API.Application.Commands.Users.ForgetPassword
{
    public sealed class ForgetUserPasswordHandler(INotificator notificator,
                                                  IAspNetIdentityService identityService) : CommandHandler<ForgetUserPasswordCommand, ForgetUserPasswordResponse>(notificator)
    {
        public override async Task<Response<ForgetUserPasswordResponse>> ExecuteAsync(ForgetUserPasswordCommand request, CancellationToken cancellationToken)
            => !ExecuteValidation(new ForgetUserPasswordValidator(), request)
            ? Response<ForgetUserPasswordResponse>.Failure(Notifications)
            : await identityService.GeneratePasswordResetTokenAsync(request).ConfigureAwait(false);
    }
}