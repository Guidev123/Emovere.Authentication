using Authentication.API.Driven.Ports.Services;
using Emovere.SharedKernel.Abstractions;
using Emovere.SharedKernel.Notifications;
using Emovere.SharedKernel.Responses;

namespace Authentication.API.Application.Commands.Users.AddRole
{
    public sealed class AddUserRoleHandler(INotificator notificator,
                                           IAspNetIdentityService identityService) : CommandHandler<AddUserRoleCommand, AddUserRoleResponse>(notificator)
    {
        public override async Task<Response<AddUserRoleResponse>> ExecuteAsync(AddUserRoleCommand request, CancellationToken cancellationToken)
            => !ExecuteValidation(new AddUserRoleValidator(), request)
                ? Response<AddUserRoleResponse>.Failure(Notifications)
                : await identityService.AddRoleToUserAsync(request).ConfigureAwait(false);
    }
}