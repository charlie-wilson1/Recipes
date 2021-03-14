export interface Recipe {
	id?: number;
	name: string;
	image?: RecipeImage;
	ingredients: Array<Ingredient>;
	instructions: Array<Instruction>;
	notes?: string;
	prepTime: number;
	cookTime: number;
	totalTime: number;
}

export interface RecipeImage {
	id?: number;
	src: string;
	fileName: string;
	fullName: string;
}

export interface Ingredient {
	id?: number;
	name: string;
	quantity: number;
	unitId: number;
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
	startNumber: number;
}
