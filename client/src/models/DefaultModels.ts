import { Units } from "@/models/Enums";
import { Ingredient, Instruction, Recipe } from "@/models/RecipeModels";

export const defaultIngredient: Ingredient = {
	id: undefined,
	name: "",
	quantity: 0,
	unitId: Units.gm,
	notes: "",
	orderNumber: 0,
};

export const defaultInstruction: Instruction = {
	id: undefined,
	orderNumber: 0,
	description: "",
};

export const defaultRecipe: Recipe = {
	id: undefined,
	name: "",
	image: undefined,
	ingredients: [],
	instructions: [],
	notes: "",
	prepTime: 0,
	cookTime: 0,
	totalTime: 0,
};
