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
	},

	highestIngredientOrderNumber(state): number {
		const highestIndex = state.recipe?.ingredients.length || 0;
		return state.recipe?.ingredients[highestIndex]?.orderNumber || 1;
	},

	highestInstructionOrderNumber(state): number {
		const highestIndex = state.recipe?.instructions.length || 0;
		return state.recipe?.instructions[highestIndex]?.orderNumber || 1;
	},
};
