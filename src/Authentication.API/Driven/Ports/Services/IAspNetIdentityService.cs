using Authentication.API.Application.Commands.Users.AddRole;
using Authentication.API.Application.Commands.Users.CreateRole;
using Authentication.API.Application.Commands.Users.Delete;
using Authentication.API.Application.Commands.Users.ForgetPassword;
using Authentication.API.Application.Commands.Users.Login;
using Authentication.API.Application.Commands.Users.Register;
using Authentication.API.Application.Commands.Users.ResetPassword;
using Authentication.API.Application.Dtos;
using Emovere.SharedKernel.Responses;

namespace Authentication.API.Driven.Ports.Services
{
    public interface IAspNetIdentityService
    {
        Task<Response<RegisterUserResponse>> RegisterAsync(RegisterUserCommand command);

        Task<Response<LoginUserResponse>> LoginAsync(LoginUserCommand command);

        Task<Response<ResetUserPasswordResponse>> ResetPasswordAsync(ResetUserPasswordCommand command);

        Task<Response<UserDto>> FindByUserEmailAsync(string email);

        Task<Response<IReadOnlyCollection<string>>> FindRolesByUserIdAsync(Guid userId);

        Task<Response<DeleteUserResponse>> DeleteAsync(DeleteUserCommand command);

        Task<Response<ForgetUserPasswordResponse>> GeneratePasswordResetTokenAsync(ForgetUserPasswordCommand command);

        Task<Response<AddUserRoleResponse>> AddRoleToUserAsync(AddUserRoleCommand command);

        Task<Response<CreateRoleResponse>> CreateRoleAsync(CreateRoleCommand command);
    }
}