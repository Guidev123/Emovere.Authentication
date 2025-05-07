using FluentValidation;

namespace Authentication.API.Application.Commands.Users.ResetPassword
{
    public sealed class ResetUserPasswordValidator : AbstractValidator<ResetUserPasswordCommand>
    {
        public ResetUserPasswordValidator()
        {
        }
    }
}