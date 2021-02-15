using System;

namespace Recipes.Application.Contracts
{
    public interface IDateTime
    {
        DateTime Now { get; set; }
        DateTime UtcNow { get; set; }
    }
}
