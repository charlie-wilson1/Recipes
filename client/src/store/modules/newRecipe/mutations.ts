import { MutationTree } from "vuex";
import { NewRecipeState } from "./state";
import {
	defaultRecipe,
	defaultIngredient,
	defaultInstruction,
	defaultImage,
} from "@/models/DefaultModels";
import {
	Ingredient,
	Instruction,
	Recipe,
	RecipeImage,
} from "@/models/RecipeModels";
import Vue from "vue";

/* eslint @typescript-eslint/no-non-null-assertion: "off" */
export const mutations: MutationTree<NewRecipeState> = {
	createNewRecipe(state) {
		state.recipe = { ...defaultRecipe };
		state.recipe.ingredients = [];
		state.recipe.instructions = [];
	},

	setRecipeById(state, recipe: Recipe) {
		state.recipe = recipe;
	},

	createDefaultIngredient(state) {
		state.selectedIngredient = { ...defaultIngredient };
	},

	createDefaultInstruction(state) {
		state.selectedInstruction = { ...defaultInstruction };
	},

	setSelectedIngredient(state, index: number) {
		state.selectedIngredient = state.recipe!.ingredients[index];
	},

	setSelectedInstruction(state, index: number) {
		state.selectedInstruction = state.recipe!.instructions[index];
	},

	insertRecipe(state, recipe: Recipe) {
		state.recipe = recipe;
		Vue.$toast.success("Saved successfully");
	},

	insertIngredient(state, ingredient: Ingredient) {
		state.recipe!.ingredients.push(ingredient);
	},

	insertInstruction(state, instruction: Instruction) {
		state.recipe!.instructions.push(instruction);
	},

	uploadRecipeImage(state, image: RecipeImage) {
		if (!state.recipe!.image) {
			state.recipe!.image = defaultImage;
		}

		state.recipe!.image!.fileName = image.fileName;
		state.recipe!.image!.src = image.src;
	},

	updteRecipe(state, recipe: Recipe) {
		state.recipe = recipe;
		Vue.$toast.success("Successfully saved recipe.");
	},

	updateIngredient(state, payload: { ingredient: Ingredient; index: number }) {
		state.recipe!.ingredients[payload.index] = payload.ingredient;
	},

	updateInstruction(
		state,
		payload: { instruction: Instruction; index: number }
	) {
		state.recipe!.instructions[payload.index] = payload.instruction;
		state.selectedInstruction = defaultInstruction;
	},

	removeIngredient(state, index: number) {
		state.recipe!.ingredients.splice(index, 1);
		const ingredientsToEdit = state.recipe!.ingredients.slice(index);

		ingredientsToEdit.forEach(
			item => (item.orderNumber = item.orderNumber - 1)
		);
	},

	removeInstruction(state, index: number) {
		state.recipe!.instructions.splice(index, 1);
		const instructionsToEdit = state.recipe!.instructions.slice(index);

		instructionsToEdit.forEach(
			item => (item.orderNumber = item.orderNumber - 1)
		);
	},

	destroyNewRecipe(state) {
		state.selectedIngredient = undefined;
		state.selectedInstruction = undefined;
		state.recipe = undefined;
	},

	deleteImage(state) {
		state.recipe!.image = undefined;
		Vue.$toast.success("Successfully deleted image.");
	},
};
