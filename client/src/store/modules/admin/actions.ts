import { ActionTree } from "vuex";
import { AdminState } from "./state";
import { RootState } from "@/store/state";
import Vue from "vue";
import axios from "axios";
import {
	AdminRegisterUserCommand,
	AdminResetUserPasswordCommand,
	UpdateRolesCommand,
	User,
} from "@/models/AdministratorModels";

const adminUrl = process.env.VUE_APP_IDENTITY_URL + "admin";

export const actions: ActionTree<AdminState, RootState> = {
	async getUsers({ commit }) {
		axios
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

	async getRoles({ commit }) {
		axios
			.get(`${adminUrl}/roles`)
			.then(response => {
				const roles: Array<string> = response.data;
				commit("setRoles", roles);
			})
			.catch(err => {
				console.log(err);
				Vue.$toast.error("Could not find any roles. Please try again.");
			});
	},

	async registerNewUser(_, command: AdminRegisterUserCommand) {
		axios.post(adminUrl + "Register", command).catch(err => {
			Vue.$toast.error(`Error registering new user: ${err}`);
		});
	},

	async updateRoles({ commit }, command: UpdateRolesCommand) {
		axios
			.patch(`${adminUrl}/UpdateRoles`, {
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

	async resetUserPassword(_, command: AdminResetUserPasswordCommand) {
		axios.post(adminUrl + "Register", command).catch(err => {
			Vue.$toast.error(`Error registering new user: ${err}`);
		});
	},
};
