using Recipes.Domain.Entities.Generic;
using System.Collections.Generic;

namespace Recipes.Domain.Entities.Recipes
{
    public class RecipeEntity : AuditableEntity
    {
        public string Name { get; set; }
        public int PrepTime { get; set; }
        public int CookTime { get; set; }
        public int? ImageId { get; set; }

        public virtual RecipeImageEntity Image { get; set; }
        public virtual ICollection<IngredientEntity> Ingredients { get; set; }
        public virtual ICollection<InstructionEntity> Instructions { get; set; }
        public virtual ICollection<RecipeNoteEntity> Notes { get; set; }
        public virtual ICollection<RecipeUser> SharedWithUsers { get; set; }
    }
}
