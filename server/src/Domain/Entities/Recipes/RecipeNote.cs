using Recipes.Domain.Entities.Generic;

namespace Recipes.Domain.Entities.Recipes
{
    public class RecipeNote : AuditableEntity
    {
        public string Description { get; set; }
        public int RecipeId { get; set; }

        public virtual Recipe Recipe { get; set; }
    }
}
