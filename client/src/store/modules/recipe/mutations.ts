import { Recipe } from "@/models/RecipeModels";
import { MutationTree } from "vuex";
import { RecipeState } from "./state";

/* eslint @typescript-eslint/no-non-null-assertion: "off" */
export const mutations: MutationTree<RecipeState> = {
	setRecipeList(state, recipeList: Recipe[]) {
		state.currentRecipeList = recipeList;
	},

	setTotalCount(state, recipeCount: number) {
		state.recipeCount = recipeCount;
	},

	setSelectedRecipe(state, recipe: Recipe) {
		state.selectedRecipe = recipe;
	},

	setRecipe(state, recipe: Recipe) {
		state.selectedRecipe = recipe;
	},

	destroyRecipeList(state) {
		state.selectedRecipe = undefined;
		state.currentRecipeList = [];
	},
};
