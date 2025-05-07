using Authentication.API.Application.Dtos;

namespace Authentication.API.Application.Commands.Users.Login
{
    public record LoginUserResponse(string AccessToken, Guid RefreshToken, double ExpiresIn, UserTokenDto Token);
}