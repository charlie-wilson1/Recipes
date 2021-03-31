using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Recipes.Identity.Application.Contracts.Services;
using Recipes.Identity.Application.Identity.Responses;
using Recipes.Identity.Domain;
using Recipes.Identity.Infrastructure.Loaders.SettingsModels;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Recipes.Identity.Infrastructure.Services
{
    public class JwtService : IJwtService
    {
        private readonly JwtBearerTokenSettings _jwtBearerTokenSettings;

        public JwtService(IOptions<JwtBearerTokenSettings> jwtBearerTokenSettings)
        {
            _jwtBearerTokenSettings = jwtBearerTokenSettings.Value;
        }

        public JwtAuthResponse GenerateJwtTokens(ApplicationUser user)
        {
            var accessToken = WriteJwtToken(user);
            var newRefreshToken = GenerateRandomToken();
            return CreateJwtResponse(accessToken, newRefreshToken);
        }

        public JwtAuthResponse CreateJwtResponse(string accessToken, string newRefreshToken)
        {
            return new JwtAuthResponse
            {
                AccessToken = accessToken,
                RefreshToken = newRefreshToken
            };
        }

        public string WriteJwtToken(ApplicationUser applicationUser)
        {
            var key = Encoding.ASCII.GetBytes(_jwtBearerTokenSettings.SecretKey);
            List<Claim> allClaims = GetClaims(applicationUser);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(allClaims),
                Expires = DateTime.UtcNow.AddMinutes(_jwtBearerTokenSettings.ExpiryTimeInSeconds),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Audience = _jwtBearerTokenSettings.Audience,
                Issuer = _jwtBearerTokenSettings.Issuer,
                IssuedAt = DateTime.UtcNow,
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public string GenerateRandomToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        private static List<Claim> GetClaims(ApplicationUser applicationUser)
        {
            var allClaims = new List<Claim>();
            allClaims.Add(new Claim(ClaimTypes.NameIdentifier, applicationUser.Id));
            allClaims.Add(new Claim(ClaimTypes.Name, applicationUser.Username));
            allClaims.Add(new Claim(ClaimTypes.Email, applicationUser.Email));

            foreach (var role in applicationUser.Roles)
            {
                allClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            return allClaims;
        }
    }
}
