import Vue from "vue";
import { ActionTree } from "vuex";
import { RootState } from "@/store/state";
import jwt from "jsonwebtoken";
import router from "@/router/index";
import {
	UpdateCurrentUserCommand,
	TokenResponse,
	LoginRequest,
} from "@/models/AccountsModels";
import axios from "axios";

const authorizationUrl = process.env.VUE_APP_IDENTITY_URL + "authorization/";
const profileUrl = process.env.VUE_APP_IDENTITY_URL + "profile/";

export const actions: ActionTree<RootState, RootState> = {
	setIsLoading: ({ commit }, isLoading: boolean) => {
		commit("setIsLoading", isLoading);
	},

	setDidToken: ({ commit }, didToken: string) => {
		commit("setDidToken", didToken);
	},

	async getJwtToken({ commit, state }, payload: LoginRequest) {
		const didToken = state.didToken ?? payload.didToken;

		await axios
			.post(authorizationUrl + "login", {
				didToken,
			})
			.then(response => {
				const encodedToken = response.data.accessToken as string;

				// eslint-disable-next-line
				const decodedToken: any = jwt.decode(encodedToken);

				const tokenPayload: TokenResponse = {
					token: encodedToken,
					magicId: decodedToken.nameid,
					username: decodedToken.name,
					roles: decodedToken.role,
					tokenExpiration: decodedToken.exp,
					publicAddress: decodedToken.publicAddress,
				};

				commit("setUserVariables", tokenPayload);

				delete axios.defaults.headers.common["Authorization"];
				axios.defaults.headers.common[
					"Authorization"
				] = `Bearer ${encodedToken}`;

				if (payload.redirect) {
					router.push(payload.redirect);
				}
			})
			.catch(err => {
				Vue.$toast.error(`Error logging in: ${err}`);
			});
	},

	async updateUsername({ commit }, command: UpdateCurrentUserCommand) {
		await axios
			.put(profileUrl + "username", {
				username: command.username,
				email: command.email,
			})
			.then(res => {
				commit("setUsername", res.data.username);
				Vue.$toast.success("Successfully updated!");
			})
			.catch(err => {
				Vue.$toast.error(`Error updating user: ${err}`);
			});
	},

	async logout({ commit, state }) {
		await axios
			.post(authorizationUrl + "logout", { didToken: state.didToken })
			.then(_ => {
				delete axios.defaults.headers.common["Authorization"];
				commit("logout");
			})
			.catch(err => Vue.$toast.error(`Error logging out: ${err}`));
	},
};
