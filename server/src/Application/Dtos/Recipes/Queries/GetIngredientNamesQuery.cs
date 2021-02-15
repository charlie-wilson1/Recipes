using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Recipes.Application.Contracts;

namespace Recipes.Application.Dtos.Recipes.Queries
{
    public class GetIngredientNamesQuery : IRequest<List<string>>
    {
        public string SearchQuery { get; set; }

        public GetIngredientNamesQuery(string searchQuery)
        {
            SearchQuery = searchQuery;
        }

        public class Handler : IRequestHandler<GetIngredientNamesQuery, List<string>>
        {
            private readonly IApplicationDbContext _context;

            public Handler(IApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<List<string>> Handle(GetIngredientNamesQuery request, CancellationToken cancellationToken)
            {
                var ingredientNames = await _context.Ingredients
                    .Where(x => string.IsNullOrWhiteSpace(request.SearchQuery) ||
                                x.Name.Contains(request.SearchQuery))
                    .GroupBy(x => x.Name)
                    .Select(x => x.Key)
                    .ToListAsync();

                return ingredientNames;
            }
        }
    }
}
