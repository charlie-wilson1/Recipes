<template lang="html">
	<section class="ingredients-form">
		<b-form-row>
			<b-col md="7">
				<b-form-row>
					<b-col md="6">
						<b-form-group
							:class="{ 'form-group--error': $v.currentIngredient.name.$error }"
							label="Ingredients"
							label-for="ingredient-name"
						>
							<b-form-input
								class="form__input"
								type="text"
								v-model.trim="$v.currentIngredient.name.$model"
								id="ingredient-name"
								placeholder="Select Ingredients"
							>
							</b-form-input>
							<div
								class="error"
								v-if="
									!$v.currentIngredient.name.required &&
										$v.currentIngredient.name.$error
								"
							>
								Field is required
							</div>
						</b-form-group>
					</b-col>
					<b-col md="2">
						<b-form-group
							:class="{
								'form-group--error': $v.currentIngredient.quantity.$error,
							}"
						>
							<label for="ingredient-quantity">Qty</label>
							<b-form-input
								type="number"
								v-model.number="$v.currentIngredient.quantity.$model"
								id="ingredient-quantity"
								min="0"
							>
							</b-form-input>
							<div
								class="error"
								v-if="
									!$v.currentIngredient.quantity.required &&
										$v.currentIngredient.quantity.$error
								"
							>
								Field is required
							</div>
							<div
								class="error"
								v-if="
									!$v.currentIngredient.quantity.minValue &&
										$v.currentIngredient.quantity.$error
								"
							>
								Must be greater than 0
							</div>
						</b-form-group>
					</b-col>
					<b-col>
						<b-form-group
							:class="{ 'form-group--error': $v.currentIngredient.unit.$error }"
						>
							<label for="ingredient-unit">Units</label>
							<b-form-select
								id="ingredient-unit"
								v-model.trim="$v.currentIngredient.unit.$model"
								:options="units"
							>
							</b-form-select>
						</b-form-group>
					</b-col>
					<b-col md="1">
						<b-button
							class="ingredient-button"
							:disabled="$v.$invalid"
							@click="addIngredient(currentIngredient)"
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
								v-model="currentIngredient.notes"
								id="ingredient-note"
								placeholder="Start typing a note..."
							></b-form-textarea>
						</b-form-group>
					</b-col>
				</b-form-row>
			</b-col>
			<b-col v-if="_ingredients">
				<CreateList
					:values="getIngredientString(_ingredients)"
					deletable="true"
					:handle-delete="handleDelete"
					:handle-move="moveItem"
				/>
			</b-col>
		</b-form-row>
	</section>
</template>

<script lang="ts">
import { Component, Vue, Prop } from "vue-property-decorator";
import { Validate } from "vuelidate-property-decorators";
import { validationMixin } from "vuelidate";
import { required, minValue } from "vuelidate/lib/validators";
import { Units } from "@/models/Enums";
import { Ingredient } from "@/models/RecipeModels";
import { defaultIngredient } from "@/models/DefaultModels";
import CreateList from "@/components/create/CreateList.vue";

@Component({
	components: {
		CreateList,
	},
	mixins: [validationMixin],
})
export default class IngredientsForm extends Vue {
	public units = Object.keys(Units)
		.filter(unit => !isNaN(Number(unit)))
		.map(value => ({
			value,
			text: Units[parseInt(value)],
		}));

	public perPage = 10;

	@Prop({ required: false })
	_ingredients?: Array<Ingredient>;

	get ingredients(): Array<Ingredient> | [] {
		if (!this._ingredients) {
			Vue.$toast.error("Could not find selected ingredient.");
			return [];
		}

		return this._ingredients;
	}

	@Validate({
		name: { required },
		quantity: { required, minValue: minValue(1) },
		unit: { required },
	})
	get currentIngredient(): Ingredient {
		let result = this.$store.getters.selectedIngredient;

		if (!result) {
			result = defaultIngredient;
		}

		return result;
	}

	get allIngredients(): Array<Ingredient> {
		const results = this.$store.getters.allIngredients;
		if (!results) {
			return [defaultIngredient];
		}
		return results;
	}

	addIngredient(ingredient: Ingredient) {
		this.$v.$touch();

		if (this.$v.$invalid) {
			Vue.$toast.error("Please complete required fields.");
			return;
		}

		this.$store.dispatch("insertIngredient", ingredient);
		this.$store.dispatch("createDefaultIngredient");
		this.$v.$reset();
	}

	getIngredientString(ingredients: Ingredient[]) {
		console.log(ingredients.map(x => x.name));
		return ingredients.map(ingredient => {
			return {
				defaultValue: ingredient.name,
				additionalValue: `${ingredient.quantity} ${Units[ingredient.unitId]}`,
				notes: ingredient.notes,
			};
		});
	}

	getIngredient(index: number): Ingredient | undefined {
		const ingredient = this.ingredients[index];
		if (!ingredient) {
			Vue.$toast.error("Could not find selected ingredient.");
			return undefined;
		}
		return ingredient;
	}

	handleDelete(index: number) {
		if (!this.getIngredient(index)) {
			return;
		}

		this.$store.dispatch("removeIngredient", index);
	}

	moveItem(from: number, to: number) {
		const swappedItem = this.getIngredient(to);
		const item = this.getIngredient(from);

		if (!swappedItem || !item) {
			return;
		}

		item.orderNumber = to;
		swappedItem.orderNumber = from;
		this.ingredients.splice(to, 0, item);
	}

	beforeCreate() {
		this.$store.dispatch("createDefaultIngredient");
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
