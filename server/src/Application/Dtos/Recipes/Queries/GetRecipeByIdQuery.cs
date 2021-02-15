using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Recipes.Application.Common.Exceptions;
using Recipes.Application.Contracts;
using Recipes.Application.Contracts.Identity;
using Recipes.Application.Dtos.Recipes.Responses;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Recipes.Application.Dtos.Recipes.Queries
{
    public class GetRecipeByIdQuery : IRequest<RecipeResponse>
    {
        public int RecipeId { get; set; }

        public GetRecipeByIdQuery(int id)
        {
            RecipeId = id;
        }

        public class Handler : IRequestHandler<GetRecipeByIdQuery, RecipeResponse>
        {
            private readonly IApplicationDbContext _context;
            private readonly ICurrentUserService _currentUserService;
            private readonly IMapper _mapper;
            private readonly IIdentityService _identityService;

            public Handler(
                IApplicationDbContext context,
                ICurrentUserService currentUserService,
                IMapper mapper, 
                IIdentityService identityService)
            {
                _context = context;
                _currentUserService = currentUserService;
                _mapper = mapper;
                _identityService = identityService;
            }
            public async Task<RecipeResponse> Handle(GetRecipeByIdQuery request, CancellationToken cancellationToken)
            {
                var currentUserId = _currentUserService.UserId;

                var recipe = await _context.Recipes
                    .Include(x => x.Image)
                    .Include(x => x.Instructions)
                    .Include(x => x.Notes)
                    .Include(x => x.Ingredients)
                    .FirstOrDefaultAsync(recipe => !recipe.IsDeleted &&
                                                    recipe.Id == request.RecipeId &&
                                                    (recipe.CreatedByUserId == currentUserId ||
                                                    recipe.SharedWithUsers.Count(user => user.UserId == currentUserId) > 0));

                if (recipe is null)
                {
                    throw new NotFoundException();
                }

                var recipeResponse = _mapper.Map<RecipeResponse>(recipe);
                recipeResponse.CreatedByUsername = await _identityService.FindUsernameByUserIdAsync(recipe.CreatedByUserId);
                return recipeResponse;
            }
        }
    }
}
