import { GetterTree } from "vuex";
import { NewRecipeState } from "./state";
import { RootState } from "@/store/state";
import { Ingredient, Instruction, Recipe } from "@/models/RecipeModels";

export const getters: GetterTree<NewRecipeState, RootState> = {
  recipe(state): Recipe {
    return state.recipe as Recipe;
  },

  selectedIngredient(state): Ingredient {
    return state.selectedIngredient as Ingredient;
  },

  selectedInstruction(state): Instruction {
    return state.selectedInstruction as Instruction;
  }
};
