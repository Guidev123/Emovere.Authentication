using Authentication.API.Extensions;
using FluentValidation;
using System.Text.RegularExpressions;

namespace Authentication.API.Application.Commands.Users.ResetPassword
{
    public sealed class ResetUserPasswordValidator : AbstractValidator<ResetUserPasswordCommand>
    {
        public ResetUserPasswordValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage(ResponseMessages.EMAIL_CANNOT_BE_EMPTY)
                .EmailAddress()
                .WithMessage(ResponseMessages.INVALID_EMAIL_FORMAT);

            RuleFor(x => x.Password)
                           .NotEmpty().WithMessage(ResponseMessages.PASSWORD_CAN_NOT_BE_EMPTY)
                           .MinimumLength(8).WithMessage(ResponseMessages.PASSWORD_MIN_LENGTH)
                           .Must(HasUpperCase).WithMessage(ResponseMessages.PASSWORD_UPPERCASE_REQUIRED)
                           .Must(HasLowerCase).WithMessage(ResponseMessages.PASSWORD_LOWERCASE_REQUIRED)
                           .Must(HasDigit).WithMessage(ResponseMessages.PASSWORD_DIGIT_REQUIRED)
                           .Must(HasSpecialCharacter).WithMessage(ResponseMessages.PASSWORD_SPECIAL_REQUIRED);

            RuleFor(x => x.ConfirmPassword)
                .Equal(x => x.Password).WithMessage(ResponseMessages.PASSWORDS_DO_NOT_MATCH)
                .When(x => !string.IsNullOrEmpty(x.Password))
                .NotEmpty().WithMessage(ResponseMessages.PASSWORD_CAN_NOT_BE_EMPTY)
                .MinimumLength(8).WithMessage(ResponseMessages.PASSWORD_MIN_LENGTH);

            RuleFor(x => x.Token)
                .NotEmpty()
                .WithMessage(ResponseMessages.TOKEN_CANNOT_BE_EMPTY);
        }

        private static bool HasUpperCase(string password) => password.Any(char.IsUpper);

        private static bool HasSpecialCharacter(string password)
        {
            var specialCharRegex = new Regex(@"[!@#$%^&*(),.?""{}|<>]");
            return specialCharRegex.IsMatch(password);
        }

        private static bool HasLowerCase(string password) => password.Any(char.IsLower);

        private static bool HasDigit(string password) => password.Any(char.IsDigit);
    }
}