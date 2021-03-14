import { Module } from "vuex";
import { RecipeState } from "./state";
import { RootState } from "@/store/state";
import { state } from "./state";
import { getters } from "./getters";
import { actions } from "./actions";
import { mutations } from "./mutations";

export const RecipeModule: Module<RecipeState, RootState> = {
	state,
	getters,
	actions,
	mutations,
};
