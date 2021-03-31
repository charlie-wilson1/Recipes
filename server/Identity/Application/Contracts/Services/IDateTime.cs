using System;

namespace Recipes.Identity.Application.Contracts.Services
{
    public interface IDateTime
    {
        DateTime Now { get; set; }
        DateTime UtcNow { get; set; }
    }
}
