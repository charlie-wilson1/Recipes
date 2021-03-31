using System;

namespace Recipes.Core.Application.Contracts.Services
{
    public interface IDateTime
    {
        DateTime Now { get; set; }
        DateTime UtcNow { get; set; }
    }
}
