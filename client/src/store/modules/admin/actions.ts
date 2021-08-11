import { ActionTree } from "vuex";
import { AdminState } from "./state";
import { RootState } from "@/store/state";
import Vue from "vue";
import axios from "axios";
import {
	CreateUserRequest,
	UpdateRolesRequest,
	User,
} from "@/models/AdministratorModels";

const adminUrl = process.env.VUE_APP_IDENTITY_URL + "user";

export const actions: ActionTree<AdminState, RootState> = {
	async getUsers({ commit }) {
		await axios
			.get(`${adminUrl}?limit=100&offset=0`)
			.then(response => {
				const users: User[] = response.data;
				commit("setUsers", users);
			})
			.catch(err => {
				console.log(err);
				Vue.$toast.error(
					"Could not find any users or roles. Please try again."
				);
			});
	},

	async createUser(_, command: CreateUserRequest) {
		await axios.post(adminUrl, command).catch(err => {
			Vue.$toast.error(`Error inviting new user: ${err}`);
		});
	},

	async updateRoles({ commit }, command: UpdateRolesRequest) {
		await axios
			.put(`${adminUrl}/roles`, {
				email: command.email,
				roles: command.roles,
			})
			.then(response => {
				const payload = {
					roles: response.data.roles,
					username: response.data.username,
				};

				commit("updateRoles", payload);
			})
			.catch(err => {
				Vue.$toast.error(`Error updating user roles: ${err}`);
			});
	},

	async deleteUser(_, email: string) {
		await axios
			.put(`${adminUrl}/delete`, { email: email })
			.then(() => {
				Vue.$toast.success("Deleted user");
			})
			.catch(err => {
				Vue.$toast.error(`Error deleting user: ${err}`);
			});
	},
};
