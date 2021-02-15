using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MediatR;
using Recipes.Application.Contracts;
using Recipes.Application.Contracts.Identity;
using Recipes.Application.Dtos.Recipes.Responses;

namespace Recipes.Application.Dtos.Recipes.Queries
{
    public class GetAllRecipesQuery : IRequest<List<RecipeResponse>>
    {
        public PaginatedRequest Request { get; set; }

        public class Handler : IRequestHandler<GetAllRecipesQuery, List<RecipeResponse>>
        {
            private readonly IApplicationDbContext _context;
            private readonly ICurrentUserService _currentUserService;

            public Handler(IApplicationDbContext context, ICurrentUserService currentUserService)
            {
                _context = context;
                _currentUserService = currentUserService;
            }

            public async Task<List<RecipeResponse>> Handle(GetAllRecipesQuery request, CancellationToken cancellationToken)
            {
                var currentUserId = _currentUserService.UserId;
                var startNumber = request.Request.StartNumber ?? 0;

                var recipes = await _context.Recipes
                    .Include(x => x.Image)
                    .Where(recipe => (string.IsNullOrWhiteSpace(request.Request.SearchQuery) ||
                                        recipe.Name.Contains(request.Request.SearchQuery)) &&
                                     (recipe.CreatedByUserId == currentUserId ||
                                        (recipe.SharedWithUsers != null &&
                                            recipe.SharedWithUsers.Count(user => user.UserId == currentUserId) > 0)))
                    .OrderBy(recipe => recipe.CreatedByUserId == currentUserId)
                    .ThenByDescending(recipe => recipe.CreatedDate)
                    .Skip(startNumber)
                    .Take(request.Request.ResultsPerPage)
                    .Select(recipe => new RecipeResponse
                    {
                        Id = recipe.Id,
                        Name = recipe.Name,
                        ImageId = recipe.ImageId,
                        ImageUrl = recipe.Image != null ? recipe.Image.Url : string.Empty,
                        ImageName = recipe.Image != null ? recipe.Image.Name : string.Empty,
                    })
                    .ToListAsync();

                return recipes;
            }
        }
    }
}
