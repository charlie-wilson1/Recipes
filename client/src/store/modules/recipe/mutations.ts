import { Recipe } from "@/models/RecipeModels";
import { MutationTree } from "vuex";
import { RecipeState } from "./state";

/* eslint @typescript-eslint/no-non-null-assertion: "off" */
export const mutations: MutationTree<RecipeState> = {
  setRecipeList(state, recipeList: Array<Recipe>) {
    state.currentRecipeList = recipeList;
  },

  setSelectedRecipe(state, index: number) {
    state.selectedRecipe = state.currentRecipeList![index];
  },

  setRecipe(state, recipe: Recipe) {
    state.selectedRecipe = recipe;
  },

  destroyRecipeList(state) {
    state.selectedRecipe = undefined;
    state.currentRecipeList = undefined;
  }
};
