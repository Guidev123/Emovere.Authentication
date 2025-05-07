using Authentication.API.Application.Commands.Users.Login;
using Authentication.API.Application.Commands.Users.Register;
using Emovere.SharedKernel.Abstractions.Mediator;
using Emovere.SharedKernel.Responses;

namespace Authentication.API.Driving.Ports.Services
{
    public sealed class UserService(IMediatorHandler mediator) : IUserService
    {
        public async Task<Response<LoginUserResponse>> LoginAsync(LoginUserCommand command)
            => await mediator.SendCommand(command).ConfigureAwait(false);

        public async Task<Response<RegisterUserResponse>> RegisterAsync(RegisterUserCommand command)
            => await mediator.SendCommand(command).ConfigureAwait(false);
    }
}