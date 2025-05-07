using Authentication.API.Application.Commands.Users.AddRole;
using Authentication.API.Application.Commands.Users.CreateRole;
using Authentication.API.Application.Commands.Users.Delete;
using Authentication.API.Application.Commands.Users.ForgetPassword;
using Authentication.API.Application.Commands.Users.Login;
using Authentication.API.Application.Commands.Users.Register;
using Authentication.API.Application.Commands.Users.ResetPassword;
using Authentication.API.Application.Dtos;
using Authentication.API.Driven.Ports.Services;
using Authentication.API.Extensions;
using Emovere.Infrastructure.Email;
using Emovere.SharedKernel.Notifications;
using Emovere.SharedKernel.Responses;
using Microsoft.AspNetCore.Identity;

namespace Authentication.API.Driven.Adapters.Services
{
    public sealed class AspNetIdentityService(SignInManager<IdentityUser> signInManager,
                                              INotificator notificator,
                                              UserManager<IdentityUser> userManager,
                                              RoleManager<IdentityRole> roleManager,
                                              IEmailService emailService,
                                              ITokenService tokenService) : IAspNetIdentityService
    {
        public async Task<Response<LoginUserResponse>> LoginAsync(LoginUserCommand command)
        {
            var result = await signInManager.PasswordSignInAsync(command.Email, command.Password, false, true);
            if (!result.Succeeded)
            {
                notificator.HandleNotification(new(ResponseMessages.INVALID_USER_CREDENTIALS));
                return Response<LoginUserResponse>.Failure(Notifications);
            }

            if (result.IsLockedOut)
            {
                notificator.HandleNotification(new(ResponseMessages.CAN_NOT_LOGIN_NOW));
                return Response<LoginUserResponse>.Failure(Notifications);
            }

            return Response<LoginUserResponse>.Success(await tokenService.JwtGenerator(command.Email));
        }

        public async Task<Response<RegisterUserResponse>> RegisterAsync(RegisterUserCommand command)
        {
            var user = MapToUser(command);

            var result = await userManager.CreateAsync(user, command.Password);

            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    notificator.HandleNotification(new(item.Description));
                }

                return Response<RegisterUserResponse>.Failure(Notifications);
            }

            var userIdentity = await FindByUserEmailAsync(command.Email);
            if (!userIdentity.IsSuccess || userIdentity.Data is null)
            {
                notificator.HandleNotification(new(ResponseMessages.FAIL_TO_CREATE_USER));
                return Response<RegisterUserResponse>.Failure(Notifications);
            }

            return Response<RegisterUserResponse>.Success(new(userIdentity.Data.UserId), code: StatusCode.CREATED_STATUS_CODE);
        }

        public async Task<Response<DeleteUserResponse>> DeleteAsync(DeleteUserCommand command)
        {
            var user = await userManager.FindByIdAsync(command.UserId.ToString()).ConfigureAwait(false);
            if (user is null)
            {
                notificator.HandleNotification(new(ResponseMessages.USER_NOT_FOUND));
                return Response<DeleteUserResponse>.Failure(Notifications, code: StatusCode.NOT_FOUND_STATUS_CODE);
            }

            var deleteUser = await userManager.DeleteAsync(user).ConfigureAwait(false);
            if (!deleteUser.Succeeded)
            {
                notificator.HandleNotification(new(ResponseMessages.FAILED_TO_DELETE_USER));
                return Response<DeleteUserResponse>.Failure(Notifications);
            }

            return Response<DeleteUserResponse>.Success(default, code: StatusCode.NO_CONTENT_STATUS_CODE);
        }

        public Task<Response<ForgetUserPasswordResponse>> GeneratePasswordResetTokenAsync(ForgetUserPasswordCommand command)
        {
            throw new NotImplementedException();
        }

        public Task<Response<ResetUserPasswordResponse>> ResetPasswordAsync(ResetUserPasswordCommand command)
        {
            throw new NotImplementedException();
        }

        public Task<Response<AddUserRoleResponse>> AddRoleToUserAsync(AddUserRoleCommand command)
        {
            throw new NotImplementedException();
        }

        public Task<Response<CreateRoleResponse>> CreateRoleAsync(CreateRoleCommand command)
        {
            throw new NotImplementedException();
        }

        public async Task<Response<UserDto>> FindByUserEmailAsync(string email)
        {
            var user = await userManager.FindByEmailAsync(email).ConfigureAwait(false);
            if (user is null)
            {
                notificator.HandleNotification(new(ResponseMessages.USER_NOT_FOUND));
                return Response<UserDto>.Failure(Notifications, code: StatusCode.NOT_FOUND_STATUS_CODE);
            }

            return Response<UserDto>.Success(new(Guid.Parse(user.Id), user.Email ?? string.Empty));
        }

        public Task<Response<IReadOnlyCollection<string>>> FindRolesByUserIdAsync(Guid userId)
        {
            throw new NotImplementedException();
        }

        private List<string> Notifications => [.. notificator.GetNotifications().Select(x => x.Message)];

        private static IdentityUser MapToUser(RegisterUserCommand command)
        {
            return new IdentityUser
            {
                UserName = command.Email,
                Email = command.Email,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };
        }
    }
}