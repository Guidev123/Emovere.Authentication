using FluentValidation;

namespace Authentication.API.Application.Commands.Users.ForgetPassword
{
    public sealed class ForgetUserPasswordValidator : AbstractValidator<ForgetUserPasswordCommand>
    {
        public ForgetUserPasswordValidator()
        {
        }
    }
}