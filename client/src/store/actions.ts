import Vue from "vue";
import { ActionTree } from "vuex";
import { RootState } from "@/store/state";
import jwt from "jsonwebtoken";
import router from "@/router/index";
import {
	UpdateCurrentUserCommand,
	UpdatePasswordCommand,
	ConfirmResetPasswordCommand,
	TokenResponse,
	LoginRequest,
	RegisterUserCommand,
} from "@/models/AccountsModels";
import axios from "axios";

const accountsUrl = process.env.VUE_APP_IDENTITY_URL + "accounts/";

export const actions: ActionTree<RootState, RootState> = {
	setIsLoading: ({ commit }, isLoading: boolean) => {
		commit("setIsLoading", isLoading);
	},

	async register({ commit }, command: RegisterUserCommand) {
		axios
			.post(accountsUrl + "register", {
				email: command.email,
				username: command.username,
				password: command.password,
				confirmPassword: command.confirmPassword,
			})
			.then(response => {
				const token = response.data.accessToken as string;

				// eslint-disable-next-line
				const decodedToken: any = jwt.decode(token);

				const data: TokenResponse = {
					token: token,
					username: decodedToken.unique_name,
					roles: decodedToken.role,
					tokenExpiration: decodedToken.exp,
					refreshToken: response.data.refreshToken,
				};

				commit("setUserVariables", data);

				if (command.redirect) {
					router.push(command.redirect);
				} else {
					router.push("home");
				}
			})
			.catch(err => {
				Vue.$toast.error(`Error registering: ${err}`);
			});
	},

	async login({ commit }, payload: LoginRequest) {
		axios
			.post(accountsUrl + "login", {
				username: payload.command.username,
				password: payload.command.password,
			})
			.then(response => {
				const token = response.data.accessToken as string;

				// eslint-disable-next-line
				const decodedToken: any = jwt.decode(token);

				const data: TokenResponse = {
					token: token,
					username: decodedToken.unique_name,
					roles: decodedToken.role,
					tokenExpiration: decodedToken.exp,
					refreshToken: response.data.refreshToken,
				};

				commit("setUserVariables", data);

				delete axios.defaults.headers.common["Authorization"];
				axios.defaults.headers.common["Authorization"] = `Bearer ${token}`;

				if (payload.redirect) {
					router.push(payload.redirect);
				} else {
					router.push("home");
				}
			})
			.catch(err => {
				Vue.$toast.error(`Error logging in: ${err}`);
			});
	},

	async getRoles({ commit }) {
		axios
			.get(accountsUrl + "roles")
			.then(response => {
				commit("updateRoles", response);
			})
			.catch(err => {
				Vue.$toast.error(`Error refreshing roles: ${err}`);
			});
	},

	async refreshJwtToken({ commit, rootState }) {
		axios
			.post(accountsUrl + "refresh", {
				username: rootState.username,
				refreshToken: rootState.refreshToken,
			})
			.then(response => {
				const token = response.data.accessToken as string;

				// eslint-disable-next-line
				const decodedToken: any = jwt.decode(token);

				const data: TokenResponse = {
					token: token,
					username: decodedToken.unique_name,
					roles: decodedToken.role,
					tokenExpiration: decodedToken.exp,
					refreshToken: response.data.refreshToken,
				};

				commit("setUserVariables", data);
				delete axios.defaults.headers.common["Authorization"];
				axios.defaults.headers.common["Authorization"] = `Bearer ${token}`;
			})
			.catch(err => {
				Vue.$toast.error(`Error logging in: ${err}`);
			});
	},

	async updateUser(_, command: UpdateCurrentUserCommand) {
		axios
			.put(`${accountsUrl}/UpdateCurrentUser`, {
				username: command.username,
				email: command.email,
			})
			.catch(err => {
				Vue.$toast.error(`Error updating user: ${err}`);
			});
	},

	async updatePassword(_, command: UpdatePasswordCommand) {
		axios
			.patch(accountsUrl + "UpdatePassword", {
				currentPassword: command.currentPassword,
				newPassword: command.newPassword,
				newPasswordConfirmation: command.newPasswordConfirmation,
			})
			.catch(err => {
				Vue.$toast.error(`Error updating password: ${err}`);
			});
	},

	async resetPassword(_, payload: { email: string; redirectUrl: string }) {
		axios
			.post(accountsUrl + "ResetPassword", {
				email: payload.email,
				redirectUrl: payload.redirectUrl,
			})
			.then(_ =>
				Vue.$toast.success("Please check your email to reset your password")
			)
			.catch(err => {
				Vue.$toast.error(`Error resetting password: ${err}`);
			});
	},

	async confirmResetPassword(_, command: ConfirmResetPasswordCommand) {
		axios
			.post(accountsUrl + "ConfirmResetPassword", {
				email: command.email,
				resetToken: command.resetToken,
				newPassword: command.newPassword,
				newPasswordConfirmation: command.newPasswordConfirmation,
			})
			.catch(err => {
				Vue.$toast.error(`Error resetting password: ${err}`);
			});
	},

	logout({ commit }) {
		delete axios.defaults.headers.common["Authorization"];
		commit("logout");
	},
};
