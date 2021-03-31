using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Recipes.Core.Application.Common.Models;
using Recipes.Core.Application.Contracts.Repositories;
using Recipes.Core.Application.Contracts.Services;
using Recipes.Core.Domain;

namespace Recipes.Core.Application.Recipes.Queries
{
    public class GetRecipesQuery : IRequest<PaginatedResponse<Recipe>>
    {
        public PaginatedRequest Request { get; set; }

        public class Handler : IRequestHandler<GetRecipesQuery, PaginatedResponse<Recipe>>
        {
            private readonly IRecipeRepository _recipeRepository;
            private readonly ICurrentUserService _currentUserService;

            public Handler(IRecipeRepository recipeRepository, ICurrentUserService currentUserService)
            {
                _recipeRepository = recipeRepository;
                _currentUserService = currentUserService;
            }

            public async Task<PaginatedResponse<Recipe>> Handle(GetRecipesQuery request, CancellationToken cancellationToken)
            {
                var results = await _recipeRepository.GetActiveAsync(_currentUserService.Username, request.Request, cancellationToken);
                return results;
            }
        }
    }
}
