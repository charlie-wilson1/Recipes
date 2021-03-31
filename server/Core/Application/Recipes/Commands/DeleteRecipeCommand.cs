using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Recipes.Core.Application.Contracts.Repositories;
using Recipes.Core.Application.Contracts.Services;

namespace Recipes.Core.Application.Recipes.Commands
{
    public class DeleteRecipeCommand : IRequest
    {
        public DeleteRecipeCommand(string id)
        {
            Id = id;
        }

        public string Id { get; set; }

        public class Handler : IRequestHandler<DeleteRecipeCommand>
        {
            private readonly IRecipeService _recipeService;
            private readonly IRecipeRepository _recipeRepository;

            public Handler(IRecipeService recipeService, IRecipeRepository recipeRepository)
            {
                _recipeService = recipeService;
                _recipeRepository = recipeRepository;
            }

            public async Task<Unit> Handle(DeleteRecipeCommand request, CancellationToken cancellationToken)
            {
                var recipe = await _recipeService.GetByIdAsync(request.Id, cancellationToken);
                _recipeService.ValidateOwnership(recipe);
                recipe.Delete();
                await _recipeRepository.UpdateAsync(recipe, cancellationToken);
                return Unit.Value;
            }
        }
    }
}
