using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Recipes.Core.Application.Contracts.Services;

namespace Recipes.Core.Api.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            Username = httpContextAccessor.HttpContext?.User.Identity.Name;
        }

        public string Username { get; private set; }
    }
}
