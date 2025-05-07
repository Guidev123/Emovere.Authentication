using Microsoft.AspNetCore.Components.Authorization;

namespace Authentication.API.Application.Dtos
{
    public record UserTokenDto(string Id, string Email, IEnumerable<ClaimDto> Claims);
}