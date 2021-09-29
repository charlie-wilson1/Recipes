import { ShoppingCart } from "@/models/ShoppingCartModels";

export interface ShoppingCartState {
	shoppingCart?: ShoppingCart;
}

export const state: ShoppingCartState = {
	shoppingCart: undefined,
};
