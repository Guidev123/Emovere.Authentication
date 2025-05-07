using FluentValidation;

namespace Authentication.API.Application.Commands.Users.AddRole
{
    public sealed class AddUserRoleValidator : AbstractValidator<AddUserRoleCommand>
    {
        public AddUserRoleValidator()
        {
        }
    }
}