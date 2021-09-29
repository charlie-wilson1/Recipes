import { ShoppingCart } from "@/models/ShoppingCartModels";
import { MutationTree } from "vuex";
import { ShoppingCartState } from "./state";

export const mutations: MutationTree<ShoppingCartState> = {
	setShoppingCart(state, cart: ShoppingCart) {
		state.shoppingCart = cart;
	},

	destroyRecipeList(state) {
		state.shoppingCart = undefined;
	},
};
