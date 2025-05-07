using Emovere.SharedKernel.Abstractions;

namespace Authentication.API.Application.Commands.Users.ResetPassword
{
    public record ResetUserPasswordCommand(
        string Token, string Email,
        string Password, string ConfirmPassword)
        : Command<ResetUserPasswordResponse>;
}