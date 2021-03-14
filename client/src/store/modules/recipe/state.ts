import { Recipe } from "@/models/RecipeModels";

export interface RecipeState {
	currentRecipeList?: Array<Recipe>;
	selectedRecipe?: Recipe;
}

export const state: RecipeState = {
	currentRecipeList: undefined,
	selectedRecipe: undefined,
};
