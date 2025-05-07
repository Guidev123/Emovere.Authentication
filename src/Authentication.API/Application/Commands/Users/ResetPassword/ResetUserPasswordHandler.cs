using Emovere.SharedKernel.Abstractions;
using Emovere.SharedKernel.Notifications;
using Emovere.SharedKernel.Responses;

namespace Authentication.API.Application.Commands.Users.ResetPassword
{
    public sealed class ResetUserPasswordHandler(INotificator notificator) : CommandHandler<ResetUserPasswordCommand, ResetUserPasswordResponse>(notificator)
    {
        public override Task<Response<ResetUserPasswordResponse>> ExecuteAsync(ResetUserPasswordCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}