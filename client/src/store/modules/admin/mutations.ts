import { MutationTree } from "vuex";
import { AdminState } from "./state";
import { User } from "@/models/AdministratorModels";

/* eslint @typescript-eslint/no-non-null-assertion: "off" */
export const mutations: MutationTree<AdminState> = {
	setUsers(state, users: Array<User>) {
		state.users = users;
	},
};
