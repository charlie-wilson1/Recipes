using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Recipes.Core.Application.Contracts.Services;
using Recipes.Core.Domain;

namespace Recipes.Core.Application.Recipes.Queries
{
    public class GetRecipeQuery : IRequest<Recipe>
    {
        public string Id { get; set; }

        public GetRecipeQuery(string id)
        {
            Id = id;
        }

        public class Handler : IRequestHandler<GetRecipeQuery, Recipe>
        {
            private readonly IRecipeService _recipeService;

            public Handler(IRecipeService recipeService)
            {
                _recipeService = recipeService;
            }

            public async Task<Recipe> Handle(GetRecipeQuery request, CancellationToken cancellationToken)
            {
                var recipe = await _recipeService.GetByIdAsync(request.Id, cancellationToken);
                return recipe;
            }
        }
    }
}
