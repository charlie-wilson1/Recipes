import { Ingredient, Instruction, Recipe } from "@/models/RecipeModels";

export interface NewRecipeState {
	recipe?: Recipe;
	allIngredients: Array<{ value: string; text: string }>;
	selectedIngredient?: Ingredient;
	selectedInstruction?: Instruction;
}

export const state: NewRecipeState = {
	recipe: undefined,
	allIngredients: [],
	selectedIngredient: undefined,
	selectedInstruction: undefined,
};
