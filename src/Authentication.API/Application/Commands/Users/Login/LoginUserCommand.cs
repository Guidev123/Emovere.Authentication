using Emovere.SharedKernel.Abstractions;

namespace Authentication.API.Application.Commands.Users.Login
{
    public record LoginUserCommand(string Email, string Password) : Command<LoginUserResponse>;
}