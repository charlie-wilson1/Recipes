using System.Collections.Generic;

namespace Recipes.Application.Dtos
{
    public class PaginatedResponse<T> where T : class
    {
        public int TotalCount { get; set; }
        public List<T> Data { get; set; }
    }
}
