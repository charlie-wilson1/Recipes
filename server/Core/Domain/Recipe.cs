using System;
using System.Collections.Generic;
using System.Linq;

namespace Recipes.Core.Domain
{
    public class Recipe
    {
        public Recipe()
        {
            Image = new Image();
            Ingredients = new List<Ingredient>();
            Instructions = new List<Instruction>();
        }

        public Recipe(string id, string name, int prepTime, int cookTime, string notes, DateTime createdDate, DateTime? lastModifiedDate, bool isDeleted, Image image, string owner, ICollection<Ingredient> ingredients, ICollection<Instruction> instructions)
        {
            Id = id;
            Name = name;
            PrepTime = prepTime;
            CookTime = cookTime;
            Notes = notes;
            CreatedDate = createdDate;
            LastModifiedDate = lastModifiedDate;
            IsDeleted = isDeleted;
            Image = image ?? new Image();
            Owner = owner;
            Ingredients = ingredients ?? new List<Ingredient>();
            Instructions = instructions ?? new List<Instruction>();
        }

        public string Id { get; protected set; }
        public string Name { get; protected set; }
        public int PrepTime { get; protected set; }
        public int CookTime { get; protected set; }
        public int TotalTime { get => PrepTime + CookTime; }
        public string Owner { get; private set; }
        public string Notes { get; protected set; }
        public bool IsDeleted { get; protected set; }
        public virtual Image Image { get; private set; }
        public virtual ICollection<Ingredient> Ingredients { get; set; }
        public virtual ICollection<Instruction> Instructions { get; set; }
        public DateTime CreatedDate { get; protected set; }
        public DateTime? LastModifiedDate { get; protected set; }

        public void Create(
            string name,
            int prepTime,
            int cookTime,
            string notes,
            string owner,
            DateTime dateTime,
            Image image = null,
            List<Ingredient> ingredients = null,
            List<Instruction> instructions = null)
        {
            if (owner is null)
            {
                throw new ArgumentException("Cannot create recipe without owner");
            }

            Name = name;
            PrepTime = prepTime;
            CookTime = cookTime;
            Notes = notes;
            Image = image ?? new Image();
            Ingredients = ingredients ?? new List<Ingredient>();
            Instructions = instructions ?? new List<Instruction>();

            CreateAuditData(owner, dateTime);
        }

        public void Update(
            Recipe recipe,
            DateTime dateTime)
        {
            Name = recipe.Name;
            PrepTime = recipe.PrepTime;
            CookTime = recipe.CookTime;
            Notes = recipe.Notes;

            UpdateAuditData(dateTime);
        }

        public void Update(
            string name,
            int prepTime,
            int cookTime,
            string notes,
            DateTime dateTime)
        {
            Name = name;
            PrepTime = prepTime;
            CookTime = cookTime;
            Notes = notes;

            UpdateAuditData(dateTime);
        }

        public void Delete()
        {
            IsDeleted = true;
        }

        public void UpdateImage(Image image)
        {
            Image = image;
        }

        public void UpdateIngredients(List<Ingredient> ingredients)
        {
            Ingredients.Clear();
            Ingredients = ingredients;
        }

        public void UpdateInstructions(List<Instruction> instructions)
        {
            Instructions.Clear();
            Instructions = instructions;
        }

        public void Copy(Recipe recipe, string newOwner, DateTime dateTime)
        {
            Create(recipe.Name, recipe.PrepTime, recipe.CookTime, recipe.Notes, newOwner, dateTime, recipe.Image, recipe.Ingredients.ToList(), recipe.Instructions.ToList());
        }

        private void CreateAuditData(string owner, DateTime dateTime)
        {
            Owner = owner;
            CreatedDate = dateTime;
        }

        private void UpdateAuditData(DateTime dateTime)
        {
            CreatedDate = dateTime;
        }

        private void EnsureIngredients()
        {
            if (Ingredients is null)
            {
                Ingredients = new List<Ingredient>();
            }
        }

        private void EnsureInstructions()
        {
            if (Instructions is null)
            {
                Instructions = new List<Instruction>();
            }
        }
    }
}
