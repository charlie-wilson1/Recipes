import { TokenResponse } from "@/models/AccountsModels";
import { MutationTree } from "vuex";
import { RootState } from "./state";

/* eslint @typescript-eslint/no-non-null-assertion: "off" */
export const mutations: MutationTree<RootState> = {
	setIsLoading: (state, isLoading: boolean) => {
		state.isLoading = isLoading;
	},

	setIsLoggedIn: (state, isLoggedIn: boolean) => {
		state.isLoggedIn = isLoggedIn;
	},

	setDidToken: (state, didToken: string) => {
		state.didToken = didToken;
	},

	setUserVariables(state, response: TokenResponse) {
		state.token = response.token;
		state.username = response.username;
		state.email = response.email;
		state.roles = response.roles;
		state.tokenExpiration = response.tokenExpiration;
	},

	logout(state) {
		state.token = undefined;
		state.username = undefined;
		state.email = undefined;
		state.roles = undefined;
		state.tokenExpiration = undefined;
	},
};
