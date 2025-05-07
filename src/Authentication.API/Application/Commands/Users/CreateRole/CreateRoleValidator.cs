using FluentValidation;

namespace Authentication.API.Application.Commands.Users.CreateRole
{
    public sealed class CreateRoleValidator : AbstractValidator<CreateRoleCommand>
    {
        public CreateRoleValidator()
        {
        }
    }
}