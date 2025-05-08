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
                                          ILogger<DeleteUserHandler> logger,
                                          IMessageBus bus) : CommandHandler<DeleteUserCommand, DeleteUserResponse>(notificator)
    {
        public override async Task<Response<DeleteUserResponse>> ExecuteAsync(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            if (!ExecuteValidation(new DeleteUserValidator(), request))
                Response<DeleteUserResponse>.Failure(Notifications);

            var result = await identityService.DeleteAsync(request).ConfigureAwait(false);
            if (!result.IsSuccess) return result;

            await DeleteCustomerAsync(request.UserId, cancellationToken).ConfigureAwait(false);

            return result;
        }

        private async Task DeleteCustomerAsync(Guid userId, CancellationToken cancellationToken)
        {
            logger.LogInformation("Enqueuing Customer with id: {CustomerId} to delete.", userId);
            await bus.PublishAsync(new DeletedUserIntegrationEvent(userId), cancellationToken).ConfigureAwait(false);
        }
    }
}