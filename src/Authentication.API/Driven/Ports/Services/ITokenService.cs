using Authentication.API.Application.Commands.Users.Login;
using Authentication.API.Models;

namespace Authentication.API.Driven.Ports.Services
{
    public interface ITokenService
    {
        Task<LoginUserResponse> JwtGenerator(string email);

        Task<RefreshToken?> GetRefreshToken(Guid refreshToken);
    }
}