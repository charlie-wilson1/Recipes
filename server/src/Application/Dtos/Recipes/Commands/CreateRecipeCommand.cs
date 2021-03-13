using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Recipes.Application.Contracts;
using Recipes.Application.Dtos.Recipes.UpsertDtos;
using Recipes.Domain.Entities.Recipes;


namespace Recipes.Application.Dtos.Recipes.Commands
{
    public class CreateRecipeCommand : IRequest<int>
    {
        public string Name { get; set; }
        public int PrepTime { get; set; }
        public int CookTime { get; set; }
        public int? ImageId { get; set; }

        public List<CreateIngredient> Ingredients { get; set; }
        public List<CreateInstruction> Instructions { get; set; }

        public class Handler : IRequestHandler<CreateRecipeCommand, int>
        {
            private readonly IApplicationDbContext _context;

            public Handler(IApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<int> Handle(CreateRecipeCommand request, CancellationToken cancellationToken)
            {
                var ingredients = request.Ingredients
                    .Select(ingredient => new Ingredient
                    {
                        Name = ingredient.Name,
                        OrderNumber = ingredient.OrderNumber,
                        Quantity = ingredient.Quantity,
                        UnitId = ingredient.UnitId,
                        Notes = ingredient.Notes
                    }).ToList();

                var instructions = request.Instructions
                    .Select(instruction => new Instruction
                    {
                        Description = instruction.Description,
                        OrderNumber = instruction.OrderNumber,
                    }).ToList();

                var recipe = new Recipe
                {
                    Name = request.Name,
                    PrepTime = request.PrepTime,
                    CookTime = request.CookTime,
                    ImageId = request.ImageId.HasValue && 
                        request.ImageId.Value > 0 ? 
                        request.ImageId.Value : 
                        null,
                    Ingredients = ingredients,
                    Instructions = instructions,
                };

                var response = await _context.Recipes.AddAsync(recipe);
                await _context.SaveChangesAsync();
                return response.Entity.Id;
            }
        }
    }
}
