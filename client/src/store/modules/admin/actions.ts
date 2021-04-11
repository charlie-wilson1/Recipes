import { ActionTree } from "vuex";
import { AdminState } from "./state";
import { RootState } from "@/store/state";
import Vue from "vue";
import axios from "axios";
import {
	AdminRegisterUserCommand,
	UpdateRolesCommand,
	User,
} from "@/models/AdministratorModels";

const adminUrl = process.env.VUE_APP_IDENTITY_URL + "admin";

export const actions: ActionTree<AdminState, RootState> = {
	async getUsers({ commit }) {
		await axios
			.get(`${adminUrl}/users`)
			.then(response => {
				const users: Array<User> = response.data;
				commit("setUsers", users);
			})
			.catch(err => {
				console.log(err);
				Vue.$toast.error(
					"Could not find any users or roles. Please try again."
				);
			});
	},

	async inviteUser(_, command: AdminRegisterUserCommand) {
		await axios.post(adminUrl + "Invitation", command).catch(err => {
			Vue.$toast.error(`Error inviting new user: ${err}`);
		});
	},

	async updateRoles({ commit }, command: UpdateRolesCommand) {
		await axios
			.patch(`${adminUrl}/roles`, {
				username: command.username,
				roles: command.roles,
			})
			.then(response => {
				const payload = {
					roles: response.data,
					username: command.username,
				};

				commit("updateRoles", payload);
			})
			.catch(err => {
				Vue.$toast.error(`Error updating user roles: ${err}`);
			});
	},

	async deleteUser(_, username: string) {
		await axios
			.delete(`${adminUrl}/${username}`)
			.then(_ => {
				Vue.$toast.success("Deleted user");
			})
			.catch(err => {
				Vue.$toast.error(`Error deleting user: ${err}`);
			});
	},
};
