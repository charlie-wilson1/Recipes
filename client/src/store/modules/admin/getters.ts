import { GetterTree } from "vuex";
import { AdminState } from "./state";
import { RootState } from "@/store/state";
import { User } from "@/models/AdministratorModels";

export const getters: GetterTree<AdminState, RootState> = {
	users(state): Array<User> {
		return state.users as Array<User>;
	},

	allRoles(state): Array<string> {
		return state.allRoles as Array<string>;
	},
};
