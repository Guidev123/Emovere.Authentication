using Emovere.SharedKernel.Abstractions;

namespace Authentication.API.Application.Commands.Users.CreateRole
{
    public record CreateRoleCommand() : Command<CreateRoleResponse>;
}