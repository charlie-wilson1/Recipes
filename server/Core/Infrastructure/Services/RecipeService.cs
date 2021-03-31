using Recipes.Core.Application.Common.Exceptions;
using Recipes.Core.Application.Contracts.Repositories;
using Recipes.Core.Application.Contracts.Services;
using Recipes.Core.Application.Recipes.Dtos.Requests;
using Recipes.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Recipes.Core.Infrastructure.Services
{
    public class RecipeService : IRecipeService
    {
        private readonly IRecipeRepository _recipeRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IDateTime _dateTime;

        public RecipeService(IRecipeRepository recipeRepository, ICurrentUserService currentUserService, IDateTime dateTime)
        {
            _recipeRepository = recipeRepository;
            _currentUserService = currentUserService;
            _dateTime = dateTime;
        }

        public async Task<Recipe> GetByIdAsync(string id, CancellationToken cancellationToken)
        {
            if (!id.Contains("/"))
            {
                id = $"recipes/{id}";
            }

            var recipe = await _recipeRepository.GetByIdAsync(id, cancellationToken);

            if (recipe is null)
            {
                throw new NotFoundException("recipe", id);
            }

            return recipe;
        }

        public void ValidateOwnership(Recipe recipe)
        {
            if (recipe.Owner != _currentUserService.Username)
            {
                throw new UnauthorizedAccessException();
            }
        }

        public void CreateIngredients(Recipe recipe, List<IngredientRequest> requests)
        {
            var ingredients = requests
                .Select(ingredient => CreateIngredient(ingredient))
                .OrderBy(ingredient => ingredient.OrderNumber)
                .ToList();
            
            recipe.UpdateIngredients(ingredients);
        }

        public Ingredient CreateIngredient(IngredientRequest request)
        {
            var ingredient = new Ingredient();
            ingredient.Upsert(request.Name, request.Unit, request.Name, request.Quantity, request.OrderNumber);
            return ingredient;
        }

        public void CreateInstructions(Recipe recipe, List<InstructionRequest> requests)
        {
            var instructions = requests
                .Select(instruction => CreateInstruction(instruction))
                .OrderBy(instruction => instruction.OrderNumber)
                .ToList();

            recipe.UpdateInstructions(instructions);
        }

        public Instruction CreateInstruction(InstructionRequest request)
        {
            var instruction = new Instruction();
            instruction.Upsert(request.OrderNumber, request.Description);
            return instruction;
        }

        public void CreateImage(Recipe recipe, ImageRequest request)
        {
            var image = new Image();
            image.Upsert(request.Url, request.Name, _dateTime.UtcNow);
            recipe.UpdateImage(image);
        }
    }
}
