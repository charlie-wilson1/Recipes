using Recipes.Domain.Entities.Generic;

namespace Recipes.Domain.Entities.Recipes
{
    public class RecipeNoteEntity : AuditableEntity
    {
        public string Description { get; set; }
        public int RecipeId { get; set; }

        public virtual RecipeEntity Recipe { get; set; }
    }
}
