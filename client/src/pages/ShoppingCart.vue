<template>
	<div>
		<section class="loader vh-100 text-center d-flex" v-if="isLoading">
			<b-spinner label="Loading..."></b-spinner>
		</section>
		<section class="cart">
			<b-container class="cart-container">
				<h1 class="pb-4">Shopping Cart</h1>
				<b-list-group class="cart-list-group flex-container h-100 pb-5">
					<b-list-group-item
						v-for="(item, index) in shoppingCart.items"
						:key="index"
						class="cart-list-item"
					>
						<ShoppingCartItemComponent :shopping-cart-item="item" />
					</b-list-group-item>
				</b-list-group>
				<b-row>
					<b-button
						variant="danger"
						class="p-2 ml-auto"
						@click="handleClearCart"
						>Clear Cart</b-button
					>
				</b-row>
			</b-container>
		</section>
	</div>
</template>

<script lang="ts">
import { Component, Vue } from "vue-property-decorator";
import { ShoppingCart } from "@/models/ShoppingCartModels";
import ShoppingCartItemComponent from "@/components/shopping-cart/ShoppingCartListItem.vue";

@Component({
	components: {
		ShoppingCartItemComponent,
	},
})
export default class ShoppingCartPage extends Vue {
	get shoppingCart(): ShoppingCart {
		return this.$store.state.ShoppingCartModule.shoppingCart;
	}

	get isLoading() {
		return this.$store.state.isLoading;
	}

	handleClearCart(): void {
		this.$store.dispatch("clearShoppingCart");
	}

	async beforeCreate() {
		await this.$store.dispatch("setIsLoading", true);
		await this.$store.dispatch("loadShoppingCart");
		await this.$store.dispatch("setIsLoading", false);
	}
}
</script>
