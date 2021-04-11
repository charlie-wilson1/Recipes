import { ActionTree } from "vuex";
import { NewRecipeState } from "./state";
import { RootState } from "@/store/state";
import {
	Recipe,
	Ingredient,
	Instruction,
	RecipeImage,
} from "@/models/RecipeModels";
import router from "@/router/index";
import Vue from "vue";
import axios from "axios";

const recipesUrl = process.env.VUE_APP_WEB_API_URL + "recipes";

export const actions: ActionTree<NewRecipeState, RootState> = {
	createNewRecipe({ commit }) {
		commit("createNewRecipe");
	},

	async setRecipeById({ commit }, recipeId: string) {
		await axios
			.get(`${recipesUrl}/${recipeId}`)
			.then(response => {
				const recipe: Recipe = response.data;
				commit("setRecipeById", recipe);
			})
			.catch(err => {
				console.log(err);
				Vue.$toast.error("Could not find recipe. Please try again.");
			});
	},

	createDefaultIngredient({ commit }) {
		commit("createDefaultIngredient");
	},

	createDefaultInstruction({ commit }) {
		commit("createDefaultInstruction");
	},

	setSelectedIngredient({ commit }, index: number) {
		commit("setSelectedIngredient", index);
	},

	setSelectedInstruction({ commit }, index: number) {
		commit("setSelectedInstruction", index);
	},

	async insertRecipe(_, recipe: Recipe) {
		await axios
			.post(recipesUrl, {
				name: recipe.name,
				notes: recipe.notes,
				prepTime: recipe.prepTime,
				cookTime: recipe.cookTime,
				image: {
					url: recipe.image?.src,
					name: recipe.image?.fileName,
				},
				ingredients: recipe.ingredients.map(ingredient => ({
					name: ingredient.name,
					quantity: ingredient.quantity,
					unit: ingredient.unit,
					orderNumber: ingredient.orderNumber + 1,
					notes: ingredient.notes,
				})),
				instructions: recipe.instructions.map(instruction => ({
					orderNumber: instruction.orderNumber,
					description: instruction.description,
				})),
			})
			.then(payload => {
				Vue.$toast.success("Saved successfully!");
				router.push(`edit/${payload.data.id.split("/").pop()}`);
			})
			.catch(err => {
				console.log(err);
				Vue.$toast.error("Could not save recipe. Please try again.");
			});
	},

	async uploadRecipeImage({ commit }, image: File) {
		const formData = new FormData();
		formData.append("formFile", image);

		const config = {
			headers: {
				"Content-Type": "multipart/form-data",
			},
		};

		axios
			.post(`${recipesUrl}/image`, formData, config)
			.then(res => {
				const imageResponse: RecipeImage = {
					src: res.data,
					fileName: image.name,
				};

				commit("uploadRecipeImage", imageResponse);
				Vue.$toast.success("Saved successfully!");
			})
			.catch(err => {
				console.log(err);
				Vue.$toast.error("Could not save image. Please try again.");
			});
	},

	insertIngredient({ commit }, ingredient: Ingredient) {
		commit("insertIngredient", ingredient);
	},

	insertInstruction({ commit }, instruction: Instruction) {
		commit("insertInstruction", instruction);
	},

	async updateRecipe({ commit }, recipe: Recipe) {
		await axios
			.put(recipesUrl, {
				id: recipe.id,
				name: recipe.name,
				notes: recipe.notes,
				prepTime: recipe.prepTime,
				cookTime: recipe.cookTime,
				image: {
					url: recipe.image?.src,
					name: recipe.image?.fileName,
				},
				ingredients: recipe.ingredients.map(ingredient => ({
					id: ingredient.id,
					name: ingredient.name,
					quantity: ingredient.quantity,
					unit: ingredient.unit,
					orderNumber: ingredient.orderNumber + 1,
					notes: ingredient.notes,
				})),
				instructions: recipe.instructions.map(instruction => ({
					id: instruction.id,
					orderNumber: instruction.orderNumber,
					description: instruction.description,
				})),
			})
			.then(() => {
				Vue.$toast.success("Saved successfully!");
				commit("updateRecipe", recipe);
			})
			.catch(err => {
				console.log(err);
				Vue.$toast.error("Could not save recipe. Please try again.");
			});
	},

	updateInstruction(
		{ commit },
		payload: { instruction: Instruction; index: number }
	) {
		commit("updateInstruction", payload);
	},

	removeIngredient({ commit }, index: number) {
		commit("removeIngredient", index);
	},

	removeInstruction({ commit }, index: number) {
		commit("removeInstruction", index);
	},

	destroyNewRecipe({ commit }) {
		commit("destroyNewRecipe");
	},

	// async deleteImage({ commit }, fileName: string) {
	// 	try {
	// 		await storage
	// 			.ref("ajax-recipes")
	// 			.child("images")
	// 			.child(fileName)
	// 			.delete();
	// 	} catch (err) {
	// 		console.log(err);
	// 		Vue.$toast.error("Failed to delete image. Please try again.");
	// 		return;
	// 	}

	// 	await axios
	// 		.delete(`${recipesUrl}/image`, {
	// 			data: fileName,
	// 		})
	// 		.then(() => {
	// 			commit("deleteImage");
	// 			Vue.$toast.success("Deleted successfully!");
	// 		})
	// 		.catch(err => {
	// 			console.log(err);
	// 			Vue.$toast.error("Failed to delete image. Please try again.");
	// 		});
	// },
};
