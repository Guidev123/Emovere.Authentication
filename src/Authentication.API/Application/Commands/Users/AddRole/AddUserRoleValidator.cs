using Authentication.API.Extensions;
using FluentValidation;

namespace Authentication.API.Application.Commands.Users.AddRole
{
    public sealed class AddUserRoleValidator : AbstractValidator<AddUserRoleCommand>
    {
        public AddUserRoleValidator()
        {
            RuleFor(x => x.Email)
              .NotEmpty()
              .WithMessage(ResponseMessages.EMAIL_CANNOT_BE_EMPTY)
              .EmailAddress()
              .WithMessage(ResponseMessages.INVALID_EMAIL_FORMAT);

            RuleFor(x => x.RoleName)
                .NotEmpty()
                .WithMessage(ResponseMessages.ROLE_NAME_CANNOT_BE_EMPTY);
        }
    }
}