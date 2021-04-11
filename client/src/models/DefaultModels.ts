import { Units } from "@/models/Enums";
import {
	Ingredient,
	Instruction,
	Recipe,
	RecipeImage,
} from "@/models/RecipeModels";

export const defaultIngredient: Ingredient = {
	id: undefined,
	name: "",
	quantity: 0,
	unit: Units.gm,
	notes: "",
	orderNumber: 0,
};

export const defaultInstruction: Instruction = {
	id: undefined,
	orderNumber: 0,
	description: "",
};

export const defaultImage: RecipeImage = {
	src: "",
	fileName: "",
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
	owner: undefined,
};
