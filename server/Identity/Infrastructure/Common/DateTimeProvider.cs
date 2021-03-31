using Recipes.Identity.Application.Contracts.Services;
using System;

namespace Recipes.Identity.Infrastructure.Common
{
    public class DateTimeProvider : IDateTime
    {
        public DateTime Now { get; set; } = DateTime.UtcNow;
        public DateTime UtcNow { get; set; } = DateTime.UtcNow;
    }
}
