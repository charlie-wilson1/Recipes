import { Module } from "vuex";
import { NewRecipeState } from "./state";
import { RootState } from "@/store/state";
import { state } from "./state";
import { getters } from "./getters";
import { actions } from "./actions";
import { mutations } from "./mutations";

export const NewRecipeModule: Module<NewRecipeState, RootState> = {
	state,
	getters,
	actions,
	mutations,
};
