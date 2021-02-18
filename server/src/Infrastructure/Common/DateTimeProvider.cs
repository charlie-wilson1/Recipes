using Recipes.Application.Contracts;
using System;

namespace Recipes.Infrastructure.Common
{
    public class DateTimeProvider : IDateTime
    {
        public DateTime Now { get; set; } = DateTime.UtcNow;
        public DateTime UtcNow { get; set; } = DateTime.UtcNow;
    }
}
