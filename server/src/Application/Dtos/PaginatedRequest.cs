using MediatR;

namespace Recipes.Application.Dtos
{
    public class PaginatedRequest
    {
        public int? StartNumber { get; set; }
        public int ResultsPerPage { get; set; }
        public string SearchQuery { get; set; }
    }
}
