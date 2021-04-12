import { Recipe } from "@/models/RecipeModels";

export interface RecipeState {
	currentRecipeList?: Array<Recipe>;
	selectedRecipe?: Recipe;
	recipeCount: number;
}

export const state: RecipeState = {
	currentRecipeList: undefined,
	selectedRecipe: undefined,
	recipeCount: 0,
};
