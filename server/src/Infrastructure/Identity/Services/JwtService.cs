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
        private readonly string providerName;
        private readonly string refreshTokenPurpose;

        public JwtService(
            IOptions<JwtBearerTokenSettings> jwtBearerTokenSettings,
            IOptions<ProviderSettings> providerSettings,
            UserManager<IdentityUser> userManager)
        {
            _jwtBearerTokenSettings = jwtBearerTokenSettings.Value;
            _userManager = userManager;
            providerName = providerSettings.Value.Name;
            refreshTokenPurpose = providerSettings.Value.ProviderPurposes.RefreshToken;
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
            var registeredToken = await _userManager.GetAuthenticationTokenAsync(user, providerName, refreshTokenPurpose);

            if (token != registeredToken)
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

                Expires = DateTime.UtcNow.AddSeconds(_jwtBearerTokenSettings.ExpiryTimeInSeconds),
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
            await _userManager.RemoveAuthenticationTokenAsync(user, providerName, refreshTokenPurpose);
            var newRefreshToken = await _userManager.GenerateUserTokenAsync(user, providerName, refreshTokenPurpose);
            await _userManager.SetAuthenticationTokenAsync(user, providerName, refreshTokenPurpose, newRefreshToken);
            return newRefreshToken;
        }
    }
}
