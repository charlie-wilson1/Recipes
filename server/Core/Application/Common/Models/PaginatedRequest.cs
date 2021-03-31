namespace Recipes.Core.Application.Common.Models
{
    public class PaginatedRequest
    {
        public string SearchQuery { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public int SkippedResults { get; set; } = 0;
    }
}
