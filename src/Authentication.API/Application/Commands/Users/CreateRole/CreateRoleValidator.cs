using Authentication.API.Extensions;
using Authentication.API.Models;
using FluentValidation;

namespace Authentication.API.Application.Commands.Users.CreateRole
{
    public sealed class CreateRoleValidator : AbstractValidator<CreateRoleCommand>
    {
        public CreateRoleValidator()
        {
            RuleFor(x => x.RoleName)
                .NotEmpty()
                .WithMessage(ResponseMessages.ROLE_NAME_CANNOT_BE_EMPTY)
                .Must(RoleIsInEnum)
                .WithMessage(ResponseMessages.INVALID_ROLE_NAME);
        }

        private static bool RoleIsInEnum(string roleName)
               => Enum.GetNames<EUserRoles>().Any(name => name.Equals(roleName, StringComparison.OrdinalIgnoreCase));
    }
}