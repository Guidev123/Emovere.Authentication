using Emovere.SharedKernel.Abstractions;
using Emovere.SharedKernel.Notifications;
using Emovere.SharedKernel.Responses;

namespace Authentication.API.Application.Commands.Users.CreateRole
{
    public sealed class CreateRoleHandler(INotificator notificator) : CommandHandler<CreateRoleCommand, CreateRoleResponse>(notificator)
    {
        public override Task<Response<CreateRoleResponse>> ExecuteAsync(CreateRoleCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}