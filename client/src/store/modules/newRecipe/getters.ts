import { GetterTree } from "vuex";
import { NewRecipeState } from "./state";
import { RootState } from "@/store/state";
import * as _ from "lodash";

export const getters: GetterTree<NewRecipeState, RootState> = {
	highestIngredientOrderNumber(state): number {
		return _.maxBy(state.recipe?.ingredients, "orderNumber")?.orderNumber ?? 0;
	},

	highestInstructionOrderNumber(state): number {
		return _.maxBy(state.recipe?.instructions, "orderNumber")?.orderNumber ?? 0;
	},
};
