using Microsoft.AspNetCore.Identity;
using Recipes.Application.Dtos.Identity.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Recipes.Application.Contracts.Identity
{
    public interface IJwtService
    {
        Task<JwtAuthResponse> GenerateJwtTokens(IdentityUser identityUser, IList<string> roles);
        Task ValidateRefreshToken(IdentityUser user, string token);
    }
}
