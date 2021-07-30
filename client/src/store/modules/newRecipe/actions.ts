import { ActionTree } from "vuex";
import { NewRecipeState } from "./state";
import { RootState } from "@/store/state";
import {
	Recipe,
	Ingredient,
	Instruction,
	RecipeImage,
	UpdateRecipe,
} from "@/models/RecipeModels";
import router from "@/router/index";
import Vue from "vue";
import axios from "axios";
import * as _ from "lodash";

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
				commit("setRecipe", recipe);
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
				image:
					recipe.image?.src == null
						? null
						: {
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

	overrideIngredients({ commit }, ingredients: Ingredient[]) {
		commit("overrideIngredients", _.sortBy(ingredients, "orderNumber"));
	},

	insertInstruction({ commit }, instruction: Instruction) {
		commit("insertInstruction", instruction);
	},

	overrideInstructions({ commit }, instructions: Instruction[]) {
		commit("overrideInstruction", _.sortBy(instructions, "orderNumber"));
	},

	async updateRecipe({ commit }, recipe: Recipe) {
		const body: UpdateRecipe = {
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
		};

		if (!recipe.image?.fileName || !recipe.image?.src) {
			if (recipe.image?.fileName || recipe.image?.src) {
				Vue.$toast.warning("Image is incorrect. Please save your image again.");
			}

			delete body.image;
		}

		await axios
			.put(recipesUrl, body)
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
};
