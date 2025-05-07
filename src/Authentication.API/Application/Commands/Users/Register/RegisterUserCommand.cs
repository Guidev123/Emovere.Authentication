using Emovere.SharedKernel.Abstractions;

namespace Authentication.API.Application.Commands.Users.Register
{
    public record RegisterUserCommand(
        string FirstName, string LastName,
        string Email, string Document,
        DateTime BirthDate, string Password, string ConfirmPassword) : Command<RegisterUserResponse>;
}