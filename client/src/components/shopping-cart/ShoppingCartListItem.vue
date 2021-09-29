<template>
	<section class="cart-item d-flex">
		<span class="cart-item-name mr-auto align-self-center p-2">{{
			shoppingCartItem.name
		}}</span>
		<div class="align-self-center p-2 mr-2 d-flex">
			<div class="quantity-container">
				<b-form-group
					:class="{
						'form-group--error': $v.itemQuantity.$error,
					}"
				>
					<b-form-input
						class="form__input"
						v-model.number="$v.itemQuantity.$model"
						@change="handleQuantityChange"
						type="number"
						min="1"
					/>
					<div v-if="$v.itemQuantity.$error">
						<div class="error" v-if="!$v.itemQuantity.required">
							Field is required
						</div>
						<div class="error" v-if="!$v.itemQuantity.minValue">
							Field must be higher than 0.
						</div>
					</div>
				</b-form-group>
			</div>
			<span class="cart-item-unit align-self-center p-2">{{
				shoppingCartItem.unit
			}}</span>
		</div>
		<div class="align-self-center p2">
			<b-icon icon="trash" variant="danger" @click="handleDelete"></b-icon>
		</div>
	</section>
</template>

<script lang="ts">
import { Component, Prop, Vue } from "vue-property-decorator";
import {
	ShoppingCartItem,
	UpdateShoppingCartItem,
} from "@/models/ShoppingCartModels";
import { Validate } from "vuelidate-property-decorators";
import { required, minValue } from "vuelidate/lib/validators";
import { Debounce } from "vue-debounce-decorator";

@Component({})
export default class ShoppingCartItemComponent extends Vue {
	@Prop({ required: true })
	shoppingCartItem!: ShoppingCartItem;

	@Validate({
		required,
		minValue: minValue(1),
	})
	itemQuantity: number | undefined = this.shoppingCartItem.quantity;

	@Debounce(1000)
	handleQuantityChange(): void {
		console.log("did it work?");
		this.$v.$touch();

		if (this.$v.$invalid) {
			Vue.$toast.error("Please fix the quanitty values");
			return;
		}

		const payload: UpdateShoppingCartItem = {
			name: this.shoppingCartItem.name,
			quantity: this.itemQuantity,
		};

		this.$store.dispatch("updateShoppingCartItem", payload);
	}

	handleDelete(): void {
		this.$store.dispatch("removeShoppingCartItem", this.shoppingCartItem.name);
	}
}
</script>

<style lang="scss" scoped>
.cart-item {
	.cart-item-name {
		font-size: 1.5em;
	}
	.quantity-container {
		width: 100px;

		input {
			font-size: 1.2em;
		}
	}

	.form-group {
		margin: 0;
	}

	.cart-item-unit {
		font-size: 1.2em;
	}

	svg {
		cursor: pointer;
	}
}
</style>
