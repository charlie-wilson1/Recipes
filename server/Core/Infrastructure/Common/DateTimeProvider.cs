using System;
using Recipes.Core.Application.Contracts.Services;

namespace Recipes.Core.Infrastructure.Common
{
    public class DateTimeProvider : IDateTime
    {
        public DateTime Now { get; set; } = DateTime.UtcNow;
        public DateTime UtcNow { get; set; } = DateTime.UtcNow;
    }
}
