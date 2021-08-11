import Vue from "vue";
import { ActionTree } from "vuex";
import { RootState } from "@/store/state";
import jwt from "jsonwebtoken";
import router from "@/router/index";
import {
	UpdateUsernameRequest,
	TokenResponse,
	LoginRequest,
} from "@/models/AccountsModels";
import axios from "axios";
import { Magic } from "magic-sdk";
import store from "./store";

const authorizationUrl = process.env.VUE_APP_IDENTITY_URL + "authorization/";
const userUrl = process.env.VUE_APP_IDENTITY_URL + "user";
const magic = new Magic(process.env.VUE_APP_MAGIC_KEY as string);

export const actions: ActionTree<RootState, RootState> = {
	setIsLoading: ({ commit }, isLoading: boolean) => {
		commit("setIsLoading", isLoading);
	},

	async setIsLoggedIn({ commit }) {
		const isLoggedIn = await magic.user.isLoggedIn();
		commit("setIsLoggedIn", isLoggedIn);
	},

	async setDidToken({ commit }) {
		const token = await magic.user.getIdToken();
		commit("setDidToken", token);
	},

	async getJwtToken({ commit, state }, payload: LoginRequest) {
		if (payload?.triedOnce) {
			await store.dispatch("setDidToken");
		}

		const didToken = payload?.didToken ?? state.didToken;

		if ((didToken ?? "").length === 0) {
			Vue.$toast.error(`Error logging in. Please login again`);
			router.push("/login");
			return;
		}

		await axios
			.post(authorizationUrl + "login", {
				didToken,
			})
			.then(response => {
				const encodedToken = response.data as string;

				// eslint-disable-next-line
				const decodedToken: any = jwt.decode(encodedToken);

				const tokenPayload: TokenResponse = {
					token: encodedToken,
					username: decodedToken.name,
					email: decodedToken.nameid,
					roles: decodedToken.roles,
					tokenExpiration: decodedToken.exp,
				};

				commit("setUserVariables", tokenPayload);

				delete axios.defaults.headers.common["Authorization"];

				axios.defaults.headers.common[
					"Authorization"
				] = `Bearer ${encodedToken}`;

				if (payload.handleRedirect) {
					if (!tokenPayload.username) {
						router.push({ name: "profile" });
					} else if (payload?.redirect) {
						router.push(payload.redirect);
					}
				}
			})
			.catch(() => {
				// if (!payload?.triedOnce) {
				// 	payload = {
				// 		...payload,
				// 		triedOnce: true,
				// 	};

				// 	store.dispatch("getJwtToken", payload);
				// }

				Vue.$toast.error("Error logging in. Please try again.");
			});
	},

	async updateUsername(_, request: UpdateUsernameRequest) {
		await axios
			.put(`${userUrl}/username`, {
				username: request.username,
				email: request.email,
			})
			.then(async () => {
				Vue.$toast.success("Successfully updated!");
				await this.dispatch("getJwtToken");
			})
			.catch(err => {
				Vue.$toast.error(`Error updating user: ${err}`);
			});
	},

	async logout({ commit, state }) {
		await axios
			.post(authorizationUrl + "logout", { didToken: state.didToken })
			.then(() => {
				delete axios.defaults.headers.common["Authorization"];
				commit("logout");
			})
			.catch(err => Vue.$toast.error(`Error logging out: ${err}`));
	},
};
