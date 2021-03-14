import Vue from "vue";
import Vuex, { StoreOptions } from "vuex";
import createPersistedState from "vuex-persistedstate";
import { RootState } from "./state";
import { RecipeModule } from "./modules/recipe/module";
import { NewRecipeModule } from "./modules/newRecipe/module";
import { AdminModule } from "./modules/admin/module";
import { actions } from "./actions";
import { mutations } from "./mutations";

Vue.use(Vuex);

const store: StoreOptions<RootState> = {
	plugins: [createPersistedState()],
	state: {
		version: "1.0.0",
		isLoading: false,
		token: undefined,
		username: undefined,
		roles: undefined,
		tokenExpiration: undefined,
	},
	modules: {
		RecipeModule,
		NewRecipeModule,
		AdminModule,
	},
	getters: {
		version: state => {
			return state.version;
		},
		isLoading: state => {
			return state.isLoading;
		},
		roles: state => {
			return state.roles ?? [];
		},
		username: state => {
			return state.username;
		},
		token: state => {
			return state.token;
		},
		tokenExpiration: state => {
			return state.tokenExpiration;
		},
		isLoggedIn: state => {
			return !!state.token;
		},
		isAdmin: state => {
			return state.roles?.includes("Admin");
		},
	},
	mutations,
	actions,
};

export default new Vuex.Store<RootState>(store);
