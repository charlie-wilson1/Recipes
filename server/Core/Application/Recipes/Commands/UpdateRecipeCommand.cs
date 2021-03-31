using MediatR;
using Recipes.Core.Application.Contracts.Repositories;
using Recipes.Core.Application.Contracts.Services;
using Recipes.Core.Application.Recipes.Dtos.Requests;
using Recipes.Core.Domain;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Recipes.Core.Application.Recipes.Commands
{
    public class UpdateRecipeCommand : IRequest<Recipe>
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int PrepTime { get; set; }
        public int CookTime { get; set; }
        public string Notes { get; set; }

        public ImageRequest Image { get; set; }
        public List<IngredientRequest> Ingredients { get; set; }
        public List<InstructionRequest> Instructions { get; set; }

        public class Handler : IRequestHandler<UpdateRecipeCommand, Recipe>
        {
            private readonly IRecipeRepository _recipeRepository;
            private readonly IRecipeService _recipeService;
            private readonly IDateTime _dateTime;

            public Handler(IRecipeRepository recipeRepository, IRecipeService recipeService, IDateTime dateTime)
            {
                _recipeRepository = recipeRepository;
                _recipeService = recipeService;
                _dateTime = dateTime;
            }

            public async Task<Recipe> Handle(UpdateRecipeCommand request, CancellationToken cancellationToken)
            {
                var recipe = await UpdateRecipeAsync(request, cancellationToken);
                await _recipeRepository.UpdateAsync(recipe, cancellationToken);
                return recipe;
            }

            private void UpsertLinkingProperties(Recipe recipe, UpdateRecipeCommand request)
            {
                _recipeService.CreateIngredients(recipe, request.Ingredients);
                _recipeService.CreateInstructions(recipe, request.Instructions);

                if (request.Image is not null)
                {
                    _recipeService.CreateImage(recipe, request.Image);
                }
            }

            public async Task<Recipe> UpdateRecipeAsync(UpdateRecipeCommand request, CancellationToken cancellationToken)
            {
                var recipe = await _recipeService.GetByIdAsync(request.Id, cancellationToken);
                _recipeService.ValidateOwnership(recipe);

                recipe.Update(
                    request.Name,
                    request.PrepTime,
                    request.CookTime,
                    request.Notes,
                    _dateTime.Now);

                UpsertLinkingProperties(recipe, request);

                return recipe;
            }
        }
    }
}
