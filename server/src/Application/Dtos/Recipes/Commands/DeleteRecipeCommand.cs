using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Recipes.Application.Common.Exceptions;
using Recipes.Application.Contracts;

namespace Recipes.Application.Dtos.Recipes.Commands
{
    public class DeleteRecipeCommand : IRequest
    {
        public int Id { get; set; }

        public class Handler : IRequestHandler<DeleteRecipeCommand>
        {
            private readonly IApplicationDbContext _context;

            public Handler(IApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(DeleteRecipeCommand request, CancellationToken cancellationToken)
            {
                var recipe = _context.Recipes.Find(request.Id);
                
                if (recipe is null)
                {
                    throw new NotFoundException();
                }

                DeleteIngredients(request.Id);
                DeleteInstructions(request.Id);
                DeleteNotes(request.Id);

                recipe.IsDeleted = true;
                _context.Recipes.Update(recipe);

                await _context.SaveChangesAsync();
                return Unit.Value;
            }

            public void DeleteIngredients(int recipeId)
            {
                var ingredients = _context.Ingredients.Where(x => !x.IsDeleted && x.RecipeId == recipeId);
                
                foreach (var ingredient in ingredients)
                {
                    ingredient.IsDeleted = true;
                }

                _context.Ingredients.UpdateRange(ingredients);
            }

            public void DeleteInstructions(int recipeId)
            {
                var instructions = _context.Instructions.Where(x => !x.IsDeleted && x.RecipeId == recipeId);

                foreach (var instruction in instructions)
                {
                    instruction.IsDeleted = true;
                }

                _context.Instructions.UpdateRange(instructions);
            }

            public void DeleteNotes(int recipeId)
            {
                var notes = _context.RecipeNotes.Where(x => !x.IsDeleted && x.RecipeId == recipeId);

                foreach (var note in notes)
                {
                    note.IsDeleted = true;
                }

                _context.RecipeNotes.UpdateRange(notes);
            }
        }
    }
}
