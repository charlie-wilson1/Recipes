import { GetterTree } from "vuex";
import { ShoppingCartState } from "./state";
import { RootState } from "@/store/state";
import { ShoppingCart, ShoppingCartItem } from "@/models/ShoppingCartModels";

export const getters: GetterTree<ShoppingCartState, RootState> = {
	shoppingCart(state): ShoppingCart | undefined {
		return state.shoppingCart ?? undefined;
	},

	shoppingCartItems(state): ShoppingCartItem[] {
		return state.shoppingCart!.items ?? [];
	},

	shoppingCartItemCount(state): number {
		return state.shoppingCart!.items?.length ?? 0;
	},
};
