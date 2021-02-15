using Recipes.Application.Contracts;
using System;

namespace Recipes.Infrastructure.Persistence.Services
{
    public class DateTimeProvider : IDateTime
    {
        public DateTime Now { get; set; } = DateTime.UtcNow;
        public DateTime UtcNow { get; set; } = DateTime.UtcNow;
    }
}
