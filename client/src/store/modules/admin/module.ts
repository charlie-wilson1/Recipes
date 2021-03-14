import { Module } from "vuex";
import { AdminState } from "./state";
import { RootState } from "@/store/state";
import { state } from "./state";
import { getters } from "./getters";
import { actions } from "./actions";
import { mutations } from "./mutations";

export const AdminModule: Module<AdminState, RootState> = {
	state,
	getters,
	actions,
	mutations,
};
