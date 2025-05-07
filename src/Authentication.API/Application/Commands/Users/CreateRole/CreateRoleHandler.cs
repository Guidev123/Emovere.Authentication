using Authentication.API.Driven.Ports.Services;
using Emovere.SharedKernel.Abstractions;
using Emovere.SharedKernel.Notifications;
using Emovere.SharedKernel.Responses;

namespace Authentication.API.Application.Commands.Users.CreateRole
{
    public sealed class CreateRoleHandler(INotificator notificator,
                                          IAspNetIdentityService identityService) : CommandHandler<CreateRoleCommand, CreateRoleResponse>(notificator)
    {
        public override async Task<Response<CreateRoleResponse>> ExecuteAsync(CreateRoleCommand request, CancellationToken cancellationToken)
            => !ExecuteValidation(new CreateRoleValidator(), request)
                ? Response<CreateRoleResponse>.Failure(Notifications)
                : await identityService.CreateRoleAsync(request);
    }
}