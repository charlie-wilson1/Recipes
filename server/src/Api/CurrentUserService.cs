using Microsoft.AspNetCore.Http;
using Recipes.Application.Contracts.Identity;
using System.Linq;
using System.Security.Claims;

namespace Recipes.WebApi
{
    public class CurrentUserService : ICurrentUserService
    {
        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            Token = httpContextAccessor.HttpContext?.Request.Headers["Authorization"]
                .FirstOrDefault()?
                .Split(" ")
                .Last();

            UserId = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            Username = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Name);
        }

        public string Token { get; set; }
        public string UserId { get; set; }
        public string Username { get; set; }
    }
}