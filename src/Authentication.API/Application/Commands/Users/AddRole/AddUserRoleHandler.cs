using Emovere.SharedKernel.Abstractions;
using Emovere.SharedKernel.Notifications;
using Emovere.SharedKernel.Responses;

namespace Authentication.API.Application.Commands.Users.AddRole
{
    public sealed class AddUserRoleHandler(INotificator notificator) : CommandHandler<AddUserRoleCommand, AddUserRoleResponse>(notificator)
    {
        public override Task<Response<AddUserRoleResponse>> ExecuteAsync(AddUserRoleCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}