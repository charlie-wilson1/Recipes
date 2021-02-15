using Microsoft.EntityFrameworkCore;
using Recipes.Domain.Entities.Recipes;
using System.Threading;
using System.Threading.Tasks;

namespace Recipes.Application.Contracts
{
    public interface IApplicationDbContext
    {
        DbSet<IngredientEntity> Ingredients { get; set; }
        DbSet<InstructionEntity> Instructions { get; set; }
        DbSet<RecipeEntity> Recipes { get; set; }
        DbSet<RecipeImageEntity> RecipeImages { get; set; }
        DbSet<RecipeNoteEntity> RecipeNotes { get; set; }
        DbSet<RecipeUser> RecipesUsers { get; set; }
        DbSet<UnitEntity> Units { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
