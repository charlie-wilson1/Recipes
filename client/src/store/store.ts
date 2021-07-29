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
		isLoggedIn: false,
		token: undefined,
		email: undefined,
		username: undefined,
		roles: [],
		tokenExpiration: undefined,
	},
	modules: {
		RecipeModule,
		NewRecipeModule,
		AdminModule,
	},
	getters: {
		isTokenExpired: state => {
			return state.tokenExpiration
				? state.tokenExpiration <= Math.floor(Date.now() / 1000)
				: true;
		},
		isAdmin: state => {
			return state.roles?.includes("Administrator");
		},
	},
	mutations,
	actions,
};

export default new Vuex.Store<RootState>(store);
