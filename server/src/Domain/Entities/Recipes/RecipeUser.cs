using Recipes.Domain.Entities.Generic;

namespace Recipes.Domain.Entities.Recipes
{
    // shared with users
    public class RecipeUser : AuditableEntity
    {
        public int RecipeId { get; set; }
        public string UserId { get; set; }

        public RecipeEntity Recipe { get; set; }
    }
}
