using Emovere.SharedKernel.Abstractions;
using Emovere.SharedKernel.Notifications;
using Emovere.SharedKernel.Responses;

namespace Authentication.API.Application.Commands.Users.ForgetPassword
{
    public sealed class ForgetUserPasswordHandler(INotificator notificator) : CommandHandler<ForgetUserPasswordCommand, ForgetUserPasswordResponse>(notificator)
    {
        public override Task<Response<ForgetUserPasswordResponse>> ExecuteAsync(ForgetUserPasswordCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}