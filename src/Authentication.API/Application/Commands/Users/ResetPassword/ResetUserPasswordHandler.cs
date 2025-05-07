using Authentication.API.Driven.Ports.Services;
using Emovere.SharedKernel.Abstractions;
using Emovere.SharedKernel.Notifications;
using Emovere.SharedKernel.Responses;

namespace Authentication.API.Application.Commands.Users.ResetPassword
{
    public sealed class ResetUserPasswordHandler(INotificator notificator,
                                                 IAspNetIdentityService identityService) : CommandHandler<ResetUserPasswordCommand, ResetUserPasswordResponse>(notificator)
    {
        public override async Task<Response<ResetUserPasswordResponse>> ExecuteAsync(ResetUserPasswordCommand request, CancellationToken cancellationToken)
            => !ExecuteValidation(new ResetUserPasswordValidator(), request)
            ? Response<ResetUserPasswordResponse>.Failure(Notifications)
            : await identityService.ResetPasswordAsync(request).ConfigureAwait(false);
    }
}