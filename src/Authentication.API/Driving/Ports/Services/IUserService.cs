using Authentication.API.Application.Commands.Users.AddRole;
using Authentication.API.Application.Commands.Users.CreateRole;
using Authentication.API.Application.Commands.Users.Delete;
using Authentication.API.Application.Commands.Users.ForgetPassword;
using Authentication.API.Application.Commands.Users.Login;
using Authentication.API.Application.Commands.Users.Register;
using Authentication.API.Application.Commands.Users.ResetPassword;
using Emovere.SharedKernel.Responses;

namespace Authentication.API.Driving.Ports.Services
{
    public interface IUserService
    {
        Task<Response<RegisterUserResponse>> RegisterAsync(RegisterUserCommand command);

        Task<Response<LoginUserResponse>> LoginAsync(LoginUserCommand command);

        Task<Response<ResetUserPasswordResponse>> ResetPasswordAsync(ResetUserPasswordCommand command);

        Task<Response<ForgetUserPasswordResponse>> ForgetPasswordAsync(ForgetUserPasswordCommand command);

        Task<Response<DeleteUserResponse>> DeleteAsync(DeleteUserCommand command);

        Task<Response<CreateRoleResponse>> CreateRoleAsync(CreateRoleCommand command);

        Task<Response<AddUserRoleResponse>> AddRoleAsync(AddUserRoleCommand command);
    }
}