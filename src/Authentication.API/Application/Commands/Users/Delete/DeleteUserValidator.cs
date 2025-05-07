using Authentication.API.Extensions;
using FluentValidation;

namespace Authentication.API.Application.Commands.Users.Delete
{
    public sealed class DeleteUserValidator : AbstractValidator<DeleteUserCommand>
    {
        public DeleteUserValidator()
        {
            RuleFor(c => c.UserId)
                .NotEqual(Guid.Empty)
                .WithMessage(ResponseMessages.INVALID_USER_ID);
        }
    }
}