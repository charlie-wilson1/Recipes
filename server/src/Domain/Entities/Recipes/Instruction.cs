using Recipes.Domain.Entities.Generic;
using System.ComponentModel.DataAnnotations;

namespace Recipes.Domain.Entities.Recipes
{
    public class Instruction : AuditableEntity
    {
        [Range(1, int.MaxValue, ErrorMessage = "must be greater than 0")]
        public int OrderNumber { get; set; }

        public string Description { get; set; }
        public int RecipeId { get; set; }

        public virtual Recipe Recipe { get; set; }
    }
}