using Emovere.SharedKernel.Abstractions;
using Emovere.SharedKernel.Notifications;
using Emovere.SharedKernel.Responses;

namespace Authentication.API.Application.Commands.Users.Delete
{
    public sealed class DeleteUserHandler(INotificator notificator) : CommandHandler<DeleteUserCommand, DeleteUserResponse>(notificator)
    {
        public override Task<Response<DeleteUserResponse>> ExecuteAsync(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}