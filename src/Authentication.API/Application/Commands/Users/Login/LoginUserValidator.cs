using Authentication.API.Extensions;
using FluentValidation;
using System.Text.RegularExpressions;

namespace Authentication.API.Application.Commands.Users.Login
{
    public sealed class LoginUserValidator : AbstractValidator<LoginUserCommand>
    {
        public LoginUserValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress().WithMessage(ResponseMessages.INVALID_EMAIL_FORMAT);

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage(ResponseMessages.PASSWORD_CAN_NOT_BE_EMPTY);
        }
    }
}