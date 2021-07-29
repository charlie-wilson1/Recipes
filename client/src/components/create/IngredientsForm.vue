<template lang="html">
	<section class="ingredients-form">
		<b-form-row>
			<b-col md="6">
				<b-form-group
					:class="{ 'form-group--error': $v.ingredient.name.$error }"
					label="Ingredients"
					label-for="ingredient-name"
				>
					<b-form-input
						class="form__input"
						type="text"
						v-model.trim="$v.ingredient.name.$model"
						id="ingredient-name"
						placeholder="Select Ingredients"
					>
					</b-form-input>
					<div
						class="error"
						v-if="!$v.ingredient.name.required && $v.ingredient.name.$error"
					>
						Field is required
					</div>
				</b-form-group>
			</b-col>
			<b-col md="2">
				<b-form-group
					:class="{
						'form-group--error': $v.ingredient.quantity.$error,
					}"
				>
					<label for="ingredient-quantity">Qty</label>
					<b-form-input
						type="number"
						v-model.number="$v.ingredient.quantity.$model"
						id="ingredient-quantity"
						min="0"
					>
					</b-form-input>
					<div
						class="error"
						v-if="
							!$v.ingredient.quantity.required && $v.ingredient.quantity.$error
						"
					>
						Field is required
					</div>
					<div
						class="error"
						v-if="
							!$v.ingredient.quantity.minValue && $v.ingredient.quantity.$error
						"
					>
						Must be greater than 0
					</div>
				</b-form-group>
			</b-col>
			<b-col>
				<b-form-group
					:class="{ 'form-group--error': $v.ingredient.unit.$error }"
				>
					<label for="ingredient-unit">Units</label>
					<b-form-select
						id="ingredient-unit"
						v-model="$v.ingredient.unit.$model"
						:options="units"
					>
					</b-form-select>
				</b-form-group>
			</b-col>
			<b-col md="1">
				<b-button
					class="ingredient-button"
					:disabled="$v.$invalid"
					@click="addIngredient"
					>Add
				</b-button>
			</b-col>
		</b-form-row>
		<b-form-row>
			<b-col>
				<b-form-group>
					<label for="ingredient-note">Ingredient Notes</label>
					<b-form-textarea
						type="text"
						v-model="ingredient.notes"
						id="ingredient-note"
						placeholder="Start typing a note..."
					></b-form-textarea>
				</b-form-group>
			</b-col>
		</b-form-row>
	</section>
</template>

<script lang="ts">
import { Component, Vue } from "vue-property-decorator";
import { Validate } from "vuelidate-property-decorators";
import { validationMixin } from "vuelidate";
import { required, minValue } from "vuelidate/lib/validators";
import { Units } from "@/models/Enums";
import { Ingredient } from "@/models/RecipeModels";
import CreateList from "@/components/create/CreateList.vue";
import { defaultIngredient } from "@/models/DefaultModels";

@Component({
	components: {
		CreateList,
	},
	mixins: [validationMixin],
})
export default class IngredientsForm extends Vue {
	@Validate({
		name: { required },
		quantity: {
			required,
			minValue: minValue(1),
		},
		unit: { required },
	})
	ingredient: Ingredient = { ...defaultIngredient };

	get units() {
		return Object.keys(Units).map(value => ({
			value,
			text: value,
		}));
	}

	addIngredient() {
		this.$v.$touch();

		if (this.$v.$invalid) {
			Vue.$toast.error("Please complete required fields.");
			return;
		}

		this.ingredient.orderNumber =
			this.$store.getters.highestIngredientOrderNumber + 1;
		this.$store.dispatch("insertIngredient", this.ingredient);
		this.$v.$reset();
		this.ingredient = { ...defaultIngredient };
	}
}
</script>

<style lang="scss">
.ingredients-form {
	.ingredient-button {
		margin-top: 30px;
	}
}
</style>
