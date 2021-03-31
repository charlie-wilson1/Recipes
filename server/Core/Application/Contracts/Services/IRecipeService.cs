using Recipes.Core.Application.Recipes.Dtos.Requests;
using Recipes.Core.Domain;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Recipes.Core.Application.Contracts.Services
{
    public interface IRecipeService
    {
        void CreateImage(Recipe recipe, ImageRequest request);
        Ingredient CreateIngredient(IngredientRequest request);
        void CreateIngredients(Recipe recipe, List<IngredientRequest> requests);
        Instruction CreateInstruction(InstructionRequest request);
        void CreateInstructions(Recipe recipe, List<InstructionRequest> requests);
        Task<Recipe> GetByIdAsync(string id, CancellationToken cancellationToken);
        void ValidateOwnership(Recipe recipe);
    }
}
