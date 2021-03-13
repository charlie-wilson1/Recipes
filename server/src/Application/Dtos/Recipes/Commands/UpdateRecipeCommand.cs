using MediatR;
using Recipes.Application.Common.Exceptions;
using Recipes.Application.Contracts;
using Recipes.Application.Contracts.Identity;
using Recipes.Application.Contracts.RecipeRepositories;
using Recipes.Application.Dtos.Recipes.UpsertDtos;
using Recipes.Domain.Entities.Generic;
using Recipes.Domain.Entities.Recipes;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Recipes.Application.Dtos.Recipes.Commands
{
    public class UpdateRecipeCommand : IRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int PrepTime { get; set; }
        public int CookTime { get; set; }
        public int? ImageId { get; set; }

        public List<UpdateIngredient> Ingredients { get; set; }
        public List<UpdateInstruction> Instructions { get; set; }

        public class Handler : IRequestHandler<UpdateRecipeCommand>
        {
            private readonly IApplicationDbContext _context;
            private readonly ICurrentUserService _currentUserService;
            private readonly IGenericAuditableEntityRepository<AuditableEntity> _genericEntityRepository;

            public Handler(
                IApplicationDbContext context,
                ICurrentUserService currentUserService,
                IGenericAuditableEntityRepository<AuditableEntity> genericEntityRepository)
            {
                _context = context;
                _currentUserService = currentUserService;
                _genericEntityRepository = genericEntityRepository;
            }

            public async Task<MediatR.Unit> Handle(UpdateRecipeCommand request, CancellationToken cancellationToken)
            {
                var currentUserId = _currentUserService.UserId;
                var recipe = _context.Recipes
                    .FirstOrDefault(x => x.Id == request.Id &&
                                         !x.IsDeleted &&
                                         x.CreatedByUserId == currentUserId);

                if (recipe is null)
                {
                    throw new NotFoundException();
                }

                // list properties update
                SaveIngredientChanges(request.Ingredients, request.Id);
                SaveInstructionChanges(request.Instructions, request.Id);

                // basic props
                recipe.CookTime = request.CookTime;
                recipe.PrepTime = request.PrepTime;
                recipe.ImageId = request.ImageId.HasValue && request.ImageId.Value > 0 ? request.ImageId.Value : null;
                recipe.Name = request.Name;

                _context.Recipes.Update(recipe);
                await _context.SaveChangesAsync();

                return MediatR.Unit.Value;
            }

            // Ingredients section
            private void SaveIngredientChanges(List<UpdateIngredient> requestedIngredients, int recipeId)
            {
                // GET LISTS
                // get entities to update/delete
                var requestedIngredientsWithId = requestedIngredients
                    .Where(x => x.Id.HasValue &&
                                x.Id.Value > default(int));

                var ingredientsToAdd = requestedIngredients
                    .Except(requestedIngredientsWithId)
                    .ToList();

                var ingredientEntities = _context.Ingredients
                    .Where(x => x.RecipeId == recipeId && !x.IsDeleted)
                    .ToList();

                var ingredientsToDelete = ingredientEntities
                    .Where(entity => requestedIngredientsWithId.All(req => req.Id != entity.Id))
                    .ToList();

                var ingredientsToEdit = ingredientEntities
                    .Except(ingredientsToDelete)
                    .ToList();

                // UPDATE ENTITIES
                _genericEntityRepository.DeleteAuditableEntities(ingredientsToDelete, false);
                EditIngredients(requestedIngredients, ingredientsToEdit);
                AddIngredients(ingredientsToAdd, recipeId);
            }

            private void AddIngredients(List<UpdateIngredient> ingredientsToAdd, int recipeId)
            {
                var ingredientEntitiesToAdd = new List<Ingredient>();

                foreach (var ingredient in ingredientsToAdd)
                {
                    ingredientEntitiesToAdd.Add(new Ingredient
                    {
                        RecipeId = recipeId,
                        Name = ingredient.Name,
                        OrderNumber = ingredient.OrderNumber,
                        Quantity = ingredient.Quantity,
                        UnitId = ingredient.UnitId,
                        Notes = ingredient.Notes
                    });
                }

                if (ingredientEntitiesToAdd.Any())
                {
                    _context.Ingredients.AddRange(ingredientEntitiesToAdd);
                }
            }

            private void EditIngredients(List<UpdateIngredient> requestedIngredients, List<Ingredient> ingredientsToEdit)
            {
                foreach (var ingredient in ingredientsToEdit)
                {
                    var requestedUpdate = requestedIngredients.Find(x => x.Id == ingredient.Id);
                    ingredient.Name = requestedUpdate.Name;
                    ingredient.OrderNumber = requestedUpdate.OrderNumber;
                    ingredient.Quantity = requestedUpdate.Quantity;
                    ingredient.UnitId = requestedUpdate.UnitId;
                }

                if (ingredientsToEdit.Any())
                {
                    _context.Ingredients.UpdateRange(ingredientsToEdit);
                }
            }

            private void DeleteIngredients(List<Ingredient> ingredientsToDelete)
            {
                foreach (var ingredient in ingredientsToDelete)
                {
                    ingredient.IsDeleted = true;
                }

                if (ingredientsToDelete.Any())
                {
                    _context.Ingredients.UpdateRange(ingredientsToDelete);
                }
            }

            // Instructions Section
            private void SaveInstructionChanges(List<UpdateInstruction> requestedInstructions, int recipeId)
            {
                var requestedInstructionsWithId = requestedInstructions
                    .Where(x => x.Id.HasValue &&
                                x.Id.Value > default(int));

                var instructionsToAdd = requestedInstructions
                    .Except(requestedInstructionsWithId)
                    .ToList();

                var instructionEntities = _context.Instructions
                    .Where(x => x.RecipeId == recipeId && !x.IsDeleted)
                    .ToList();

                var instructionsToDelete = instructionEntities
                    .Where(entity => !requestedInstructionsWithId.All(req => req.Id == entity.Id))
                    .ToList();

                var instructionsToEdit = instructionEntities
                    .Except(instructionsToDelete)
                    .ToList();

                _genericEntityRepository.DeleteAuditableEntities(instructionsToDelete, false);
                EditInstructions(requestedInstructions, instructionsToEdit);
                AddInstructions(instructionsToAdd, recipeId);
            }

            private void AddInstructions(List<UpdateInstruction> instructionsToAdd, int recipeId)
            {
                var instructionEntitiesToAdd = new List<Instruction>();

                foreach (var instruction in instructionsToAdd)
                {
                    instructionEntitiesToAdd.Add(new Instruction
                    {
                        RecipeId = recipeId,
                        OrderNumber = instruction.OrderNumber,
                        Description = instruction.Description
                    });
                }

                if (instructionEntitiesToAdd.Any())
                {
                    _context.Instructions.AddRange(instructionEntitiesToAdd);
                }
            }

            private void EditInstructions(List<UpdateInstruction> requestedInstructions, List<Instruction> instructionsToEdit)
            {
                foreach (var instruction in instructionsToEdit)
                {
                    var requestedUpdate = requestedInstructions.Find(x => x.Id == instruction.Id);
                    instruction.OrderNumber = requestedUpdate.OrderNumber;
                    instruction.Description = requestedUpdate.Description;
                }

                if (instructionsToEdit.Any())
                {
                    _context.Instructions.UpdateRange(instructionsToEdit);
                }
            }

            private void DeleteInstructions(List<Instruction> instructionsToDelete)
            {
                foreach (var instruction in instructionsToDelete)
                {
                    instruction.IsDeleted = true;
                }

                if (instructionsToDelete.Any())
                {
                    _context.Instructions.UpdateRange(instructionsToDelete);
                }
            }
        }
    }
}
