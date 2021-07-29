import { ActionTree } from "vuex";
import { RecipeState } from "./state";
import { RootState } from "@/store/state";
import { GetAllRecipesQuery, Recipe } from "@/models/RecipeModels";
import Vue from "vue";
import axios from "axios";

const recipesUrl = process.env.VUE_APP_WEB_API_URL + "recipes";

export const actions: ActionTree<RecipeState, RootState> = {
	async loadRecipeList({ commit }, query: GetAllRecipesQuery) {
		await axios
			.get(recipesUrl, {
				params: {
					pageSize: query.resultsPerPage,
					pageNumber: query.pageNumber,
					searchQuery: query.searchQuery,
				},
			})
			.then(response => {
				const recipes: Array<Recipe> = response.data.data;
				commit("setRecipeList", recipes);
				commit("setTotalCount", response.data.total);
				commit("setRecipe", recipes[0]);
			})
			.catch(err => {
				console.log(err);
				Vue.$toast.error("Could not get recipes. Please try again.");
			});
	},

	async getRecipeById({ commit }, id: string) {
		await axios
			.get(`${recipesUrl}/${id}`)
			.then(response => {
				const recipe: Recipe = response.data;
				commit("setRecipe", recipe);
			})
			.catch(err => {
				console.log(err);
				Vue.$toast.error("Could not get recipe. Please try again.");
			});
	},

	setSelectedRecipe({ commit }, recipe: Recipe) {
		commit("setSelectedRecipe", recipe);
	},

	destroyRecipeList({ commit }) {
		commit("destroyRecipeList");
	},
};
