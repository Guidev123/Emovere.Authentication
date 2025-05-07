using Authentication.API.Application.Commands.Users.Login;
using Authentication.API.Application.Dtos;
using Authentication.API.Driven.Adapters.Data;
using Authentication.API.Driven.Ports.Services;
using Authentication.API.Extensions;
using Authentication.API.Models;
using KeyPairJWT.Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Authentication.API.Driven.Adapters.Services
{
    public sealed class TokenService(UserManager<IdentityUser> userManager,
                                     IOptions<AppTokenSettings> tokenSettings,
                                     IJwtService jwtService,
                                     AuthenticationDbContext dbContext,
                                     IHttpContextAccessor httpContextAccessor) : ITokenService
    {
        private readonly AppTokenSettings _tokenSettings = tokenSettings.Value;

        public async Task<LoginUserResponse> JwtGenerator(string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            var claims = await userManager.GetClaimsAsync(user!);

            var identityClaims = await GetUserClaims(claims, user!);
            var encodedToken = await EncodingToken(identityClaims);

            var refreshToken = await GenerateRefreshToken(email);

            return GetTokenResponse(encodedToken, user!, claims, refreshToken);
        }

        public async Task<RefreshToken?> GetRefreshToken(Guid refreshToken)
        {
            var token = await dbContext.RefreshTokens.AsNoTracking().FirstOrDefaultAsync(rt => rt.Token == refreshToken);

            return token is not null && token.ExpirationDate > DateTime.Now ? token : null;
        }

        #region Private Methods

        private async Task<RefreshToken> GenerateRefreshToken(string email)
        {
            var refreshToken = new RefreshToken(email, DateTime.Now.AddHours(_tokenSettings.RefreshTokenExpirationInHours));

            dbContext.RefreshTokens.RemoveRange(dbContext.RefreshTokens.Where(rt => rt.UserIdentification == email));
            await dbContext.RefreshTokens.AddAsync(refreshToken);
            await dbContext.SaveChangesAsync();

            return refreshToken;
        }

        private async Task<ClaimsIdentity> GetUserClaims(ICollection<Claim> claims, IdentityUser user)
        {
            var userRoles = await userManager.GetRolesAsync(user);

            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id));
            claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty));
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Nbf, ToUnixEpochDate(DateTime.UtcNow).ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(DateTime.UtcNow).ToString(), ClaimValueTypes.Integer64));
            foreach (var userRole in userRoles)
            {
                claims.Add(new Claim("role", userRole));
            }

            var identityClaims = new ClaimsIdentity();
            identityClaims.AddClaims(claims);

            return identityClaims;
        }

        private async Task<string> EncodingToken(ClaimsIdentity identityClaims)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            string currentIssuer = $"{httpContextAccessor.HttpContext?.Request.Scheme}://{httpContextAccessor.HttpContext?.Request.Host}";

            var key = await jwtService.GetCurrentSigningCredentials();
            var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = currentIssuer,
                Subject = identityClaims,
                Expires = DateTime.UtcNow.AddHours(_tokenSettings.TokenExpirationInHours),
                SigningCredentials = key
            });

            return tokenHandler.WriteToken(token);
        }

        private static long ToUnixEpochDate(DateTime date)
             => (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);

        private static LoginUserResponse GetTokenResponse(string encodedToken, IdentityUser user, IEnumerable<Claim> claims, RefreshToken refreshToken)
            => new(encodedToken, refreshToken.Token, TimeSpan.FromHours(1).TotalSeconds,
                new(user.Id, user.Email ?? string.Empty, claims.Select(c => new ClaimDto(c.Type, c.Value))));

        #endregion Private Methods
    }
}