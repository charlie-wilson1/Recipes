using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Recipes.Core.Application.Common.Models;
using Recipes.Core.Application.Contracts.Repositories;
using Recipes.Core.Application.Contracts.Services;
using Recipes.Core.Domain;

namespace Recipes.Core.Application.Recipes.Queries
{
    public class GetRecipesQuery : IRequest<PaginatedResponse<Recipe>>
    {
        public GetRecipesQuery(string searchQuery, int pageSize, int pageNumber, int skippedResults)
        {
            SearchQuery = searchQuery;
            PageSize = pageSize;
            PageNumber = pageNumber;
            SkippedResults = skippedResults;
        }

        public string SearchQuery { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public int SkippedResults { get; set; } = 0;

        public class Handler : IRequestHandler<GetRecipesQuery, PaginatedResponse<Recipe>>
        {
            private readonly IRecipeRepository _recipeRepository;
            private readonly ICurrentUserService _currentUserService;
            private readonly IMapper _mapper;

            public Handler(
                IRecipeRepository recipeRepository,
                ICurrentUserService currentUserService,
                IMapper mapper)
            {
                _recipeRepository = recipeRepository;
                _currentUserService = currentUserService;
                _mapper = mapper;
            }

            public async Task<PaginatedResponse<Recipe>> Handle(GetRecipesQuery request, CancellationToken cancellationToken)
            {
                var paginatedRequest = _mapper.Map<PaginatedRequest>(request);
                var results = await _recipeRepository.GetActiveAsync(_currentUserService.Username, paginatedRequest, cancellationToken);
                return results;
            }
        }
    }
}
