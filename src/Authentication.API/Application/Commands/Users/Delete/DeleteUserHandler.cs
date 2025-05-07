using Authentication.API.Driven.Ports.Services;
using Emovere.Communication.IntegrationEvents;
using Emovere.Infrastructure.Bus;
using Emovere.SharedKernel.Abstractions;
using Emovere.SharedKernel.Notifications;
using Emovere.SharedKernel.Responses;

namespace Authentication.API.Application.Commands.Users.Delete
{
    public sealed class DeleteUserHandler(INotificator notificator,
                                          IAspNetIdentityService identityService,
                                          IMessageBus bus) : CommandHandler<DeleteUserCommand, DeleteUserResponse>(notificator)
    {
        public override async Task<Response<DeleteUserResponse>> ExecuteAsync(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            if (!ExecuteValidation(new DeleteUserValidator(), request))
                Response<DeleteUserResponse>.Failure(Notifications);

            var result = await identityService.DeleteAsync(request).ConfigureAwait(false);
            if (!result.IsSuccess) return result;

            await bus.PublishAsync(new DeletedUserIntegrationEvent(request.UserId), cancellationToken).ConfigureAwait(false);

            return result;
        }
    }
}