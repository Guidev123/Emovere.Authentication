using Emovere.SharedKernel.Abstractions;

namespace Authentication.API.Application.Commands.Users.ForgetPassword
{
    public record ForgetUserPasswordCommand() : Command<ForgetUserPasswordResponse>;
}