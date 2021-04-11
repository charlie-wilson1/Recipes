import { MutationTree } from "vuex";
import { AdminState } from "./state";
import { User } from "@/models/AdministratorModels";
import Vue from "vue";

/* eslint @typescript-eslint/no-non-null-assertion: "off" */
export const mutations: MutationTree<AdminState> = {
	setUsers(state, users: Array<User>) {
		state.users = users;
	},

	updateRoles(state, payload: { roles: Array<string>; username: string }) {
		const userToUpdateIndex = state.users.findIndex(
			user => user.username === payload.username
		);

		console.log("payload", payload.username);
		console.log("user", userToUpdateIndex);

		if (userToUpdateIndex < 1) {
			Vue.$toast.error("Could not find user");
			return;
		}

		state.users[userToUpdateIndex].roles = payload.roles;
	},
};
