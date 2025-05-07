using Authentication.API.Extensions;
using FluentValidation;

namespace Authentication.API.Application.Commands.Users.ForgetPassword
{
    public sealed class ForgetUserPasswordValidator : AbstractValidator<ForgetUserPasswordCommand>
    {
        public ForgetUserPasswordValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage(ResponseMessages.EMAIL_CANNOT_BE_EMPTY)
                .EmailAddress()
                .WithMessage(ResponseMessages.INVALID_EMAIL_FORMAT);

            RuleFor(x => x.ClientUrlToResetPassword)
                .NotEmpty()
                .WithMessage(ResponseMessages.CLIENT_URL_TO_RESET_PASSWORD_CANNOT_BE_EMPTY)
                .Must(x => x.Contains("http://") || x.Contains("https://"))
                .WithMessage(ResponseMessages.CLIENT_URL_TO_RESET_PASSWORD_INVALID);
        }
    }
}