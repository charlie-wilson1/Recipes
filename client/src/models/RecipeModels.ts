export interface Recipe {
	id?: string;
	name: string;
	image?: RecipeImage;
	ingredients: Array<Ingredient>;
	instructions: Array<Instruction>;
	notes?: string;
	prepTime: number;
	cookTime: number;
	totalTime: number;
	owner?: string;
}

export interface UpdateRecipe {
	id?: string;
	name: string;
	image?: UpdateRecipeImage | null;
	ingredients: Array<Ingredient>;
	instructions: Array<Instruction>;
	notes?: string;
	prepTime: number;
	cookTime: number;
	owner?: string;
}

export interface RecipeImage {
	src: string;
	fileName: string;
}

export interface UpdateRecipeImage {
	url?: string;
	name?: string;
}

export interface Ingredient {
	id?: number;
	name: string;
	quantity: number;
	unit: string;
	notes?: string;
	orderNumber: number;
}

export interface Instruction {
	id?: number;
	orderNumber: number;
	description: string;
}

export interface GetAllRecipesQuery {
	searchQuery: string | undefined;
	resultsPerPage: number;
	pageNumber: number;
}
