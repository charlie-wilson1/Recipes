using System;
using System.Collections.Generic;

namespace Recipes.Application.Dtos.Recipes.Responses
{
    public class RecipeResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int PrepTime { get; set; }
        public int CookTime { get; set; }
        public int TotalTime { get; set; }
        public int? ImageId { get; set; }
        public string ImageUrl { get; set; }
        public string ImageName { get; set; }
        public string Notes { get; set; }
        public string CreatedByUsername { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }

        public List<InstructionDto> Instructions { get; set; }
        public List<IngredientDto> Ingredients { get; set; }
    }
}
