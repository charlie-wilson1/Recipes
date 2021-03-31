using Recipes.Identity.Application.Identity.Responses;
using Recipes.Identity.Domain;

namespace Recipes.Identity.Application.Contracts.Services
{
    public interface IJwtService
    {
        JwtAuthResponse CreateJwtResponse(string accessToken, string newRefreshToken);
        JwtAuthResponse GenerateJwtTokens(ApplicationUser user);
        string GenerateRandomToken();
        string WriteJwtToken(ApplicationUser applicationUser);
    }
}
