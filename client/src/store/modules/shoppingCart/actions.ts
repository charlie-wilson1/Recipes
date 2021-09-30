import { ActionTree } from "vuex";
import { ShoppingCartState } from "./state";
import { RootState } from "@/store/state";
import Vue from "vue";
import axios from "axios";
import {
	ShoppingCart,
	ShoppingCartItem,
	UpdateShoppingCartItem,
} from "@/models/ShoppingCartModels";

const cartUrl = process.env.VUE_APP_WEB_API_URL + "cart";

export const actions: ActionTree<ShoppingCartState, RootState> = {
	async loadShoppingCart({ commit }) {
		await axios
			.get(cartUrl)
			.then(response => {
				const cart: ShoppingCart = response.data;
				commit("setShoppingCart", cart);
			})
			.catch(err => {
				console.log(err);
				Vue.$toast.error("Could not get shopping cart. Please try again.");
			});
	},

	async createShoppingCart({ commit }) {
		await axios
			.post(cartUrl, {})
			.then(response => {
				const cart: ShoppingCart = response.data;
				commit("setShoppingCart", cart);
			})
			.catch(err => console.log(err));
	},

	async addRecipeToShoppingCart({ commit }, recipeId: string) {
		await axios
			.patch(`${cartUrl}/recipe`, recipeId)
			.then(response => {
				const cart: ShoppingCart = response.data;
				commit("setShoppingCart", cart);
				Vue.$toast.success("Added recipe ingredients to the cart");
			})
			.catch(err => {
				console.log(err);
				Vue.$toast.error("Could not get shopping cart. Please try again.");
			});
	},

	async addItemToShoppingCart({ commit }, item: ShoppingCartItem) {
		await axios
			.patch(`${cartUrl}/items/add`, { item })
			.then(response => {
				const cart: ShoppingCart = response.data;
				commit("setShoppingCart", cart);
				Vue.$toast.success("Added item to the cart");
			})
			.catch(err => {
				console.log(err);
				Vue.$toast.error("Could not update shopping cart. Please try again.");
			});
	},

	async updateShoppingCartItem({ commit }, item: UpdateShoppingCartItem) {
		await axios
			.patch(`${cartUrl}/items/update`, { item })
			.then(response => {
				const cart: ShoppingCart = response.data;
				commit("setShoppingCart", cart);
			})
			.catch(err => {
				console.log(err);
				Vue.$toast.error("Could not update shopping cart. Please try again.");
			});
	},

	async removeShoppingCartItem({ commit }, itemName: string) {
		await axios
			.patch(`${cartUrl}/items/remove`, { itemName })
			.then(response => {
				const cart: ShoppingCart = response.data;
				commit("setShoppingCart", cart);
			})
			.catch(err => {
				console.log(err);
				Vue.$toast.error("Could not update shopping cart. Please try again.");
			});
	},

	async clearShoppingCart({ commit }) {
		await axios
			.patch(`${cartUrl}/clear`, {})
			.then(response => {
				const cart: ShoppingCart = response.data;
				commit("setShoppingCart", cart);
			})
			.catch(err => {
				console.log(err);
				Vue.$toast.error("Could not clear shopping cart. Please try again.");
			});
	},

	destroyShoppingList({ commit }) {
		commit("destroyShoppingList");
	},
};
