using FluentValidation;

namespace Authentication.API.Application.Commands.Users.Delete
{
    public sealed class DeleteUserValidator : AbstractValidator<DeleteUserCommand>
    {
        public DeleteUserValidator()
        {
        }
    }
}