using Authentication.API.Application.Commands.Users.Login;
using Authentication.API.Application.Commands.Users.Register;
using Emovere.SharedKernel.Responses;

namespace Authentication.API.Driving.Ports.Services
{
    public interface IUserService
    {
        Task<Response<RegisterUserResponse>> RegisterAsync(RegisterUserCommand command);

        Task<Response<LoginUserResponse>> LoginAsync(LoginUserCommand command);
    }
}