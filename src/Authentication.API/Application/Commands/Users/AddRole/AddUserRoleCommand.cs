using Emovere.SharedKernel.Abstractions;

namespace Authentication.API.Application.Commands.Users.AddRole
{
    public record AddUserRoleCommand(string Email, string RoleName) : Command<AddUserRoleResponse>;
}