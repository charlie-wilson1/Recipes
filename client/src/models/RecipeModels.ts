import { Units } from "./Enums";

export interface Recipe {
  id: string;
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
  src: string;
  fileName: string;
  fullName: string;
}

export interface Ingredient {
  id: string;
  name: string;
  quantity: number;
  unit: Units;
  notes?: string;
  orderNumber: number;
}

export interface Instruction {
  orderNumber: number;
  description: string;
}

export interface GetAllRecipesQuery {
  searchQuery: string | undefined;
  resultsPerPage: number;
  startNumber: number;
}