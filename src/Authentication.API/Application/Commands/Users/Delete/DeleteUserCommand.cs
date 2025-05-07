using Emovere.SharedKernel.Abstractions;

namespace Authentication.API.Application.Commands.Users.Delete
{
    public record DeleteUserCommand(Guid UserId) : Command<DeleteUserResponse>;
}