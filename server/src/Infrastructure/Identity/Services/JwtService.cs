using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Recipes.Application.Contracts.Identity;
using Recipes.Application.Dtos.Identity.Responses;
using Recipes.Infrastructure.Identity.Models;

namespace Recipes.Infrastructure.Identity.Services
{
    public class JwtService : IJwtService
    {
        private readonly JwtBearerTokenSettings _jwtBearerTokenSettings;
        private readonly UserManager<IdentityUser> _userManager;

        public JwtService(IOptions<JwtBearerTokenSettings> jwtBearerTokenSettings, UserManager<IdentityUser> userManager)
        {
            _jwtBearerTokenSettings = jwtBearerTokenSettings.Value;
            _userManager = userManager;
        }

        public async Task<JwtAuthResponse> GenerateJwtTokens(IdentityUser identityUser, IList<string> roles)
        {
            var accessToken = WriteJwtToken(identityUser, roles);
            var refreshToken = await GenerateRefreshToken(identityUser);

            return new JwtAuthResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }

        public async Task ValidateRefreshToken(IdentityUser user, string token)
        {
            var refreshToken = await _userManager.GetAuthenticationTokenAsync(user, "Recipes", "RefreshToken");
            var isValid = await _userManager.VerifyUserTokenAsync(user, "Recipes", "RefreshToken", refreshToken);

            if (!isValid)
            {
                throw new SecurityTokenValidationException();
            }
        }

        private string WriteJwtToken(IdentityUser identityUser, IList<string> roles)
        {
            var key = Encoding.ASCII.GetBytes(_jwtBearerTokenSettings.SecretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, identityUser.Id),
                    new Claim(ClaimTypes.Name, identityUser.UserName),
                    new Claim(ClaimTypes.Email, identityUser.Email),
                }),

                Expires = DateTime.UtcNow.AddMinutes(_jwtBearerTokenSettings.ExpiryTimeInSeconds),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Audience = _jwtBearerTokenSettings.Audience,
                Issuer = _jwtBearerTokenSettings.Issuer,
                IssuedAt = DateTime.UtcNow,
            };

            foreach (var role in roles)
            {
                tokenDescriptor.Subject.AddClaim(new Claim(ClaimTypes.Role, role));
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var accessToken = tokenHandler.WriteToken(token);

            return accessToken;
        }

        public async Task<string> GenerateRefreshToken(IdentityUser user)
        {
            await _userManager.RemoveAuthenticationTokenAsync(user, "Recipes", "RefreshToken");
            var newRefreshToken = await _userManager.GenerateUserTokenAsync(user, "Recipes", "RefreshToken");
            await _userManager.SetAuthenticationTokenAsync(user, "Recipes", "RefreshToken", newRefreshToken);
            return newRefreshToken;
        }
    }
}
