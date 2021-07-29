import { Recipe } from "@/models/RecipeModels";

export interface RecipeState {
	currentRecipeList: Recipe[];
	selectedRecipe?: Recipe;
	recipeCount: number;
}

export const state: RecipeState = {
	currentRecipeList: [],
	selectedRecipe: undefined,
	recipeCount: 0,
};
