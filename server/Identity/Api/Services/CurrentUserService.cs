using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Recipes.Identity.Application.Contracts.Services;

namespace Recipes.Identity.Api.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            UserId = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            Username = httpContextAccessor.HttpContext?.User?.Identity?.Name;
        }

        public string UserId { get; private set; }
        public string Username { get; private set; }
    }
}
