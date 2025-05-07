using Emovere.SharedKernel.Abstractions;

namespace Authentication.API.Application.Commands.Users.ResetPassword
{
    public record ResetUserPasswordCommand() : Command<ResetUserPasswordResponse>;
}