import { Module } from "vuex";
import { ShoppingCartState } from "./state";
import { RootState } from "@/store/state";
import { state } from "./state";
import { getters } from "./getters";
import { actions } from "./actions";
import { mutations } from "./mutations";

export const ShoppingCartModule: Module<ShoppingCartState, RootState> = {
	state,
	getters,
	actions,
	mutations,
};
