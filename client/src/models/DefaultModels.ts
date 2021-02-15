import { Units } from "@/models/Enums";
import { Ingredient, Instruction, Recipe } from "@/models/RecipeModels";

export const defaultIngredient: Ingredient = {
  id: "",
  name: "",
  quantity: 0,
  unit: Units.gm,
  notes: "",
  orderNumber: 0
};

export const defaultInstruction: Instruction = {
  orderNumber: 0,
  description: ""
};

export const defaultRecipe: Recipe = {
  id: "",
  name: "",
  image: undefined,
  ingredients: [],
  instructions: [],
  notes: "",
  prepTime: 0,
  cookTime: 0,
  totalTime: 0
};
