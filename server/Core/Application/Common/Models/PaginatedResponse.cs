using System.Collections.Generic;

namespace Recipes.Core.Application.Common.Models
{
    public class PaginatedResponse<T> where T : class
    {
        public IEnumerable<T> Data { get; set; }
        public int Total { get; set; }
    }
}
