using Authentication.API.Application.Commands.Users.AddRole;
using Authentication.API.Application.Commands.Users.CreateRole;
using Authentication.API.Application.Commands.Users.Delete;
using Authentication.API.Application.Commands.Users.ForgetPassword;
using Authentication.API.Application.Commands.Users.Login;
using Authentication.API.Application.Commands.Users.Register;
using Authentication.API.Application.Commands.Users.ResetPassword;
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

        public async Task<Response<DeleteUserResponse>> DeleteAsync(DeleteUserCommand command)
            => await mediator.SendCommand(command).ConfigureAwait(false);

        public async Task<Response<ForgetUserPasswordResponse>> ForgetPasswordAsync(ForgetUserPasswordCommand command)
            => await mediator.SendCommand(command).ConfigureAwait(false);

        public async Task<Response<ResetUserPasswordResponse>> ResetPasswordAsync(ResetUserPasswordCommand command)
            => await mediator.SendCommand(command).ConfigureAwait(false);

        public async Task<Response<AddUserRoleResponse>> AddRoleAsync(AddUserRoleCommand command)
            => await mediator.SendCommand(command).ConfigureAwait(false);

        public async Task<Response<CreateRoleResponse>> CreateRoleAsync(CreateRoleCommand command)
            => await mediator.SendCommand(command).ConfigureAwait(false);
    }
}