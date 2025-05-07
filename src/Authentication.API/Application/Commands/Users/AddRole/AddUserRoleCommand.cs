using Emovere.SharedKernel.Abstractions;

namespace Authentication.API.Application.Commands.Users.AddRole
{
    public record AddUserRoleCommand() : Command<AddUserRoleResponse>;
}