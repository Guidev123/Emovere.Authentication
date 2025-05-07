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
using Authentication.API.Models;
using Emovere.Infrastructure.Email;
using Emovere.Infrastructure.Email.Models;
using Emovere.SharedKernel.Notifications;
using Emovere.SharedKernel.Responses;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;

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

        public async Task<Response<ForgetUserPasswordResponse>> GeneratePasswordResetTokenAsync(ForgetUserPasswordCommand command)
        {
            var user = await userManager.FindByEmailAsync(command.Email);
            if (user is null)
            {
                notificator.HandleNotification(new(ResponseMessages.USER_NOT_FOUND));
                return Response<ForgetUserPasswordResponse>.Failure(Notifications, code: StatusCode.NOT_FOUND_STATUS_CODE);
            }

            var token = await userManager.GeneratePasswordResetTokenAsync(user);

            var param = new Dictionary<string, string?>
            {
                {"token", token },
                {"email", command.Email}
            };

            var callback = QueryHelpers.AddQueryString(command.ClientUrlToResetPassword, param);

            var message = new EmailMessage(command.Email, "Link to reset password.", callback);
            await emailService.SendAsync(message);

            return Response<ForgetUserPasswordResponse>.Success(default, code: StatusCode.NO_CONTENT_STATUS_CODE);
        }

        public async Task<Response<ResetUserPasswordResponse>> ResetPasswordAsync(ResetUserPasswordCommand command)
        {
            var user = await userManager.FindByEmailAsync(command.Email);
            if (user is null)
            {
                notificator.HandleNotification(new(ResponseMessages.USER_NOT_FOUND));
                return Response<ResetUserPasswordResponse>.Failure(Notifications, code: StatusCode.NOT_FOUND_STATUS_CODE);
            }

            var result = await userManager.ResetPasswordAsync(user, command.Token, command.Password);
            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    notificator.HandleNotification(new(item.Description));
                }

                return Response<ResetUserPasswordResponse>.Failure(Notifications);
            }

            return Response<ResetUserPasswordResponse>.Success(default, code: StatusCode.NO_CONTENT_STATUS_CODE);
        }

        public async Task<Response<AddUserRoleResponse>> AddRoleToUserAsync(AddUserRoleCommand command)
        {
            var roleIsValid = await RoleIsValidAsync(command.RoleName);
            if (!roleIsValid)
            {
                notificator.HandleNotification(new(ResponseMessages.INVALID_ROLE_NAME));
                return Response<AddUserRoleResponse>.Failure(Notifications);
            }

            var user = await userManager.FindByEmailAsync(command.Email);
            if (user is null)
            {
                notificator.HandleNotification(new(ResponseMessages.USER_NOT_FOUND));
                return Response<AddUserRoleResponse>.Failure(Notifications, code: StatusCode.NOT_FOUND_STATUS_CODE);
            }

            var result = await userManager.AddToRoleAsync(user, command.RoleName);
            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    notificator.HandleNotification(new(item.Description));
                }
                return Response<AddUserRoleResponse>.Failure(Notifications);
            }

            return Response<AddUserRoleResponse>.Success(default, code: StatusCode.NO_CONTENT_STATUS_CODE);
        }

        public async Task<Response<CreateRoleResponse>> CreateRoleAsync(CreateRoleCommand command)
        {
            if (!RoleIsInEnum(command.RoleName))
            {
                notificator.HandleNotification(new(ResponseMessages.INEXISTENT_ROLE));
                return Response<CreateRoleResponse>.Failure(Notifications);
            }

            if (await roleManager.RoleExistsAsync(command.RoleName))
            {
                notificator.HandleNotification(new($"Role: '{command.RoleName}' already exists."));
                return Response<CreateRoleResponse>.Failure(Notifications);
            }

            var role = new IdentityRole(command.RoleName);
            var result = await roleManager.CreateAsync(role);

            if (!result.Succeeded || role.Name is null)
            {
                foreach (var error in result.Errors)
                {
                    notificator.HandleNotification(new(error.Description));
                }
                return Response<CreateRoleResponse>.Failure(Notifications);
            }

            var roleResult = new CreateRoleResponse(Guid.Parse(role.Id));
            return Response<CreateRoleResponse>.Success(roleResult, code: StatusCode.CREATED_STATUS_CODE);
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

        public async Task<Response<IReadOnlyCollection<string>>> FindRolesByUserIdAsync(Guid userId)
        {
            var user = await userManager.FindByIdAsync(userId.ToString());
            if (user is null)
            {
                notificator.HandleNotification(new(ResponseMessages.USER_NOT_FOUND));
                return Response<IReadOnlyCollection<string>>.Failure(Notifications, code: StatusCode.NOT_FOUND_STATUS_CODE);
            }

            return Response<IReadOnlyCollection<string>>.Success([.. await userManager.GetRolesAsync(user)]);
        }

        private async Task<bool> RoleIsValidAsync(string roleName)
        {
            var roleIsValid = RoleIsInEnum(roleName);

            var roleExists = await roleManager.RoleExistsAsync(roleName);

            return roleIsValid && roleExists;
        }

        private static bool RoleIsInEnum(string roleName)
            => Enum.GetNames<EUserRoles>().Any(name => name.Equals(roleName, StringComparison.OrdinalIgnoreCase));

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