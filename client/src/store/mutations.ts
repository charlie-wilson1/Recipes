import { TokenResponse } from "@/models/AccountsModels";
import { MutationTree } from "vuex";
import { RootState } from "./state";

/* eslint @typescript-eslint/no-non-null-assertion: "off" */
export const mutations: MutationTree<RootState> = {
	setIsLoading: (state, isLoading: boolean) => {
		state.isLoading = isLoading;
	},

	setDidToken: (state, didToken: string) => {
		state.didToken = didToken;
	},

	setUserVariables(state, response: TokenResponse) {
		state.token = response.token;
		state.username = response.username;
		state.roles = response.roles;
		state.tokenExpiration = response.tokenExpiration;
		state.magicId = response.magicId;
	},

	setUsername(state, response: string) {
		state.username = response;
	},

	setEmail(state, response: string) {
		state.email = response;
	},

	logout(state) {
		state.token = undefined;
		state.username = undefined;
		state.roles = undefined;
		state.tokenExpiration = undefined;
		state.magicId = undefined;
	},
};
