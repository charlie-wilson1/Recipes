using System;

namespace Recipes.Core.Application.Common.Models
{
    public class EntityUpdateDto
    {
        public string CurrentUserId { get; set; }
        public DateTime Now { get; set; }
    }
}
