using Microsoft.EntityFrameworkCore;
using Recipes.Domain.Entities.Recipes;
using System.Threading;
using System.Threading.Tasks;

namespace Recipes.Application.Contracts
{
    public interface IApplicationDbContext
    {
        DbSet<Ingredient> Ingredients { get; set; }
        DbSet<Instruction> Instructions { get; set; }
        DbSet<Recipe> Recipes { get; set; }
        DbSet<RecipeImage> RecipeImages { get; set; }
        DbSet<RecipeUser> RecipesUsers { get; set; }
        DbSet<Unit> Units { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
