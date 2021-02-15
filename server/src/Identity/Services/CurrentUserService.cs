using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Recipes.Application.Contracts.Identity;

namespace Recipes.Identity.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            UserId = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            Username = httpContextAccessor.HttpContext?.User?.Identity?.Name;
        }

        public string UserId { get; set; }
        public string Username { get; set; }
    }
}
