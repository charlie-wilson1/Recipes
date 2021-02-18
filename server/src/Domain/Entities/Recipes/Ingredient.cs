using Recipes.Domain.Entities.Generic;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Recipes.Domain.Entities.Recipes
{
    public class Ingredient : AuditableEntity
    {
        public string Name { get; set; }
        public string Notes { get; set; }
        public int UnitId { get; set; }
        public int RecipeId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "must be greater than 0")]
        public int Quantity { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "must be greater than 0")]
        public int OrderNumber { get; set; }

        public virtual Recipe Recipe { get; set; }
    }
}