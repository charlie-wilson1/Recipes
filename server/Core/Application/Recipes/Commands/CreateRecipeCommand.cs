using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Recipes.Core.Application.Contracts.Repositories;
using Recipes.Core.Application.Contracts.Services;
using Recipes.Core.Application.Recipes.Dtos.Requests;
using Recipes.Core.Domain;

namespace Recipes.Core.Application.Recipes.Commands
{
    public class CreateRecipeCommand : IRequest<Recipe>
    {
        public string Name { get; set; }
        public int PrepTime { get; set; }
        public int CookTime { get; set; }
        public string Notes { get; set; }

        public ImageRequest Image { get; set; }
        public List<IngredientRequest> Ingredients { get; set; }
        public List<InstructionRequest> Instructions { get; set; }

        public class Handler : IRequestHandler<CreateRecipeCommand, Recipe>
        {
            private readonly IRecipeRepository _recipeRepository;
            private readonly IRecipeService _recipeService;
            private readonly ICurrentUserService _currentUserService;
            private readonly IDateTime _dateTime;

            public Handler(IRecipeRepository recipeRepository, IRecipeService recipeService, ICurrentUserService currentUserService, IDateTime dateTime)
            {
                _recipeRepository = recipeRepository;
                _recipeService = recipeService;
                _currentUserService = currentUserService;
                _dateTime = dateTime;
            }

            public async Task<Recipe> Handle(CreateRecipeCommand request, CancellationToken cancellationToken)
            {
                var recipe = CreateRecipe(request);
                CreateLinkingProperties(recipe, request);
                await _recipeRepository.InsertAsync(recipe, cancellationToken);

                return recipe;
            }

            private void CreateLinkingProperties(Recipe recipe, CreateRecipeCommand request)
            {
                _recipeService.CreateIngredients(recipe, request.Ingredients);
                _recipeService.CreateInstructions(recipe, request.Instructions);
                _recipeService.CreateImage(recipe, request.Image);
            }

            public Recipe CreateRecipe(CreateRecipeCommand request)
            {
                var recipe = new Recipe();
                var owner = _currentUserService.Username;

                recipe.Create(
                    request.Name,
                    request.PrepTime,
                    request.CookTime,
                    request.Notes,
                    owner,
                    _dateTime.Now);

                return recipe;
            }
        }
    }
}
