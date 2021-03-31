<template>
	<div>
		<section class="loader vh-100 text-center d-flex" v-if="isLoading">
			<b-spinner label="Loading..."></b-spinner>
		</section>
		<section class="create-recipe container">
			<h1 class="text-center">{{ title }}</h1>
			<b-form @submit.prevent="handleSubmit(currentRecipe)">
				<b-form-row>
					<b-col md="6">
						<b-form-group
							label="Name"
							label-for="name"
							:class="{ 'form-group--error': $v.currentRecipe.name.$error }"
						>
							<b-form-input
								class="form__input"
								type="text"
								id="name"
								placeholder="Name"
								v-model.trim="$v.currentRecipe.name.$model"
							></b-form-input>
							<div
								class="error"
								v-if="
									!$v.currentRecipe.name.required &&
										$v.currentRecipe.name.$error
								"
							>
								Field is required
							</div>
						</b-form-group>
					</b-col>
					<b-col md="6">
						<b-form-group label="Image" label-for="image">
							<b-form-file
								ref="recipeImage"
								placeholder="Choose a file or drop it here..."
								drop-placeholder="Drop file here..."
								@change="handleFileUpload"
							></b-form-file>
							<span v-if="currentRecipe.image">
								<span class="mr-2">
									<b-icon
										icon="trash"
										scale="1"
										@click="handleDeleteImage(currentRecipe.image.fullName)"
									></b-icon>
								</span>
								{{ currentRecipe.image.fileName }}
							</span>
						</b-form-group>
					</b-col>
				</b-form-row>

				<b-form-row>
					<b-col md="4">
						<b-form-group
							label="Prep Time"
							label-for="prepTime"
							:class="{ 'form-group--error': $v.currentRecipe.prepTime.$error }"
						>
							<b-form-input
								class="form__input"
								v-model.number="$v.currentRecipe.prepTime.$model"
								type="number"
								id="prepTime"
								placeholder="Prep"
								min="1"
								required
							></b-form-input>
							<div
								class="error"
								v-if="
									!$v.currentRecipe.name.required &&
										$v.currentRecipe.name.$error
								"
							>
								Field is required
							</div>
						</b-form-group>
					</b-col>
					<b-col md="4">
						<b-form-group
							label="Cook Time"
							label-for="cookTime"
							:class="{ 'form-group--error': $v.currentRecipe.cookTime.$error }"
						>
							<b-form-input
								class="form__input"
								v-model.number="$v.currentRecipe.cookTime.$model"
								type="number"
								id="cookTime"
								placeholder="Cook"
								min="1"
								required
							></b-form-input>
							<div
								class="error"
								v-if="
									!$v.currentRecipe.name.required &&
										$v.currentRecipe.name.$error
								"
							>
								Field is required
							</div>
						</b-form-group>
					</b-col>
					<b-col md="4">
						<b-form-group label="Total Time" label-for="totalTime">
							<b-form-input
								:value="totalTime"
								type="text"
								id="totalTime"
								readonly
								placeholder="Total"
								min="0"
								required
							></b-form-input>
						</b-form-group>
					</b-col>
				</b-form-row>
				<hr />
				<b-form-row>
					<b-col md="7">
						<IngredientsForm />
					</b-col>
					<b-col>
						<CreateList
							:values="getIngredientString()"
							deletable="true"
							:handle-move="changeIngredientOrder"
							:handle-delete="deleteIngredient"
						/>
					</b-col>
				</b-form-row>
				<hr />
				<b-form-row>
					<b-col md="7">
						<InstructionsForm :instruction.sync="instruction" />
					</b-col>
					<b-col>
						<CreateList
							:values="getInstructionString()"
							deletable="true"
							editable="true"
							:handle-move="changeInstructionOrder"
							:handle-edit="editInstruction"
							:handle-delete="deleteInstruction"
						/>
					</b-col>
				</b-form-row>
				<hr />
				<b-form-row>
					<b-col md="12">
						<b-form-group label="Notes" label-for="notes">
							<b-form-textarea
								class="form__input"
								id="notes"
								v-model.trim="currentRecipe.notes"
							>
							</b-form-textarea>
						</b-form-group>
					</b-col>
				</b-form-row>
				<b-form-group class="text-center button-wrapper">
					<b-button type="submit" variant="success mr-2">Submit </b-button>
					<b-button variant="primary" v-b-modal.clear-modal>Clear </b-button>
				</b-form-group>
			</b-form>
		</section>
		<section class="clear-modal">
			<b-modal centered id="clear-modal" title="Clear Recipe" @ok="handleReset">
				<div class="d-block text-center">
					<p>
						Are you sure you would like to clear the recipe? Changes will not be
						saved.
					</p>
				</div>
			</b-modal>
		</section>
	</div>
</template>

<script lang="ts">
import { Component, Vue } from "vue-property-decorator";
import { Validate } from "vuelidate-property-decorators";
import { validationMixin } from "vuelidate";
import { required, minValue, minLength } from "vuelidate/lib/validators";
import { Instruction, Recipe } from "@/models/RecipeModels";
import { defaultInstruction, defaultRecipe } from "@/models/DefaultModels";
import CreateList from "@/components/create/CreateList.vue";
import IngredientsForm from "@/components/create/IngredientsForm.vue";
import InstructionsForm from "@/components/create/InstructionsForm.vue";
import { Units } from "@/models/Enums";
import { indexIsInArray } from "@/mixins/listUtils";

@Component({
	components: {
		CreateList,
		IngredientsForm,
		InstructionsForm,
	},
	mixins: [validationMixin],
})
export default class CreateRecipe extends Vue {
	instruction: Instruction = { ...defaultInstruction };

	get isEditMode(): boolean {
		return this.$route.path.toLowerCase().includes("edit");
	}

	get recipeId(): number | undefined {
		if (this.isEditMode) {
			return parseInt(this.$route.params.recipe_id);
		}
		return undefined;
	}

	get title(): string {
		return this.isEditMode ? "Edit Recipe" : "Create Recipe";
	}

	get isLoading(): boolean {
		return this.$store.getters.isLoading;
	}

	@Validate({
		name: { required },
		ingredients: {
			required,
			minLength: minLength(1),
		},
		instructions: {
			required,
			minLength: minLength(1),
		},
		prepTime: { required, minValue: minValue(1) },
		cookTime: { required, minValue: minValue(1) },
	})
	get currentRecipe(): Recipe {
		let recipe: Recipe = this.$store.getters.recipe;

		if (!recipe) {
			recipe = { ...defaultRecipe };
		}

		return recipe;
	}

	get totalTime(): number {
		return +this.currentRecipe.prepTime + +this.currentRecipe.cookTime;
	}

	deleteIngredient(index: number) {
		if (!indexIsInArray(index, this.currentRecipe.ingredients.length)) {
			this.$toast.error("Could not find selected ingredients.");
			return;
		}

		const orderNumber = this.currentRecipe.ingredients[index].orderNumber;
		this.currentRecipe.ingredients.splice(index);

		this.currentRecipe.ingredients
			.filter(i => i.orderNumber >= orderNumber)
			.map(i => ({
				...i,
				orderNumber: i.orderNumber - 1,
			}));
	}

	changeIngredientOrder(fromIndex: number, toIndex: number) {
		const ingredientListLength = this.currentRecipe.ingredients.length;

		if (
			!indexIsInArray(fromIndex, ingredientListLength) ||
			!indexIsInArray(fromIndex, ingredientListLength)
		) {
			this.$toast.error("Could not find selected ingredient.");
			return;
		}

		const fromOrderNumber = this.currentRecipe.ingredients[fromIndex]
			.orderNumber;
		const toOrderNumber = this.currentRecipe.ingredients[toIndex].orderNumber;

		this.currentRecipe.ingredients[fromIndex].orderNumber = toOrderNumber;
		this.currentRecipe.ingredients[toIndex].orderNumber = fromOrderNumber;

		this.currentRecipe.ingredients = this.currentRecipe.ingredients.sort(
			i => i.orderNumber
		);
	}

	deleteInstruction(index: number) {
		if (!indexIsInArray(index, this.currentRecipe.instructions.length)) {
			this.$toast.error("Could not find selected instruction.");
			return;
		}

		const orderNumber = this.currentRecipe.instructions[index].orderNumber;
		this.currentRecipe.instructions.splice(index);

		this.currentRecipe.instructions
			.filter(i => i.orderNumber >= orderNumber)
			.map(i => ({
				...i,
				orderNumber: i.orderNumber - 1,
			}));
	}

	editInstruction(index: number) {
		console.log(index);
		if (!indexIsInArray(index, this.currentRecipe.instructions.length)) {
			this.$toast.error("Could not find selected instruction.");
			return;
		}

		this.instruction = this.currentRecipe.instructions[index];
	}

	changeInstructionOrder(fromIndex: number, toIndex: number) {
		const instructionListLength = this.currentRecipe.instructions.length;

		if (
			!indexIsInArray(fromIndex, instructionListLength) ||
			!indexIsInArray(fromIndex, instructionListLength)
		) {
			this.$toast.error("Could not find selected instruction.");
			return;
		}

		const fromOrderNumber = this.currentRecipe.instructions[fromIndex]
			.orderNumber;
		const toOrderNumber = this.currentRecipe.instructions[toIndex].orderNumber;

		this.currentRecipe.instructions[fromIndex].orderNumber = toOrderNumber;
		this.currentRecipe.instructions[toIndex].orderNumber = fromOrderNumber;

		this.currentRecipe.instructions = this.currentRecipe.instructions.sort(
			i => i.orderNumber
		);
	}

	getIngredientString() {
		return (this.currentRecipe.ingredients || []).map(ingredient => {
			return {
				defaultValue: ingredient.name,
				additionalValue: `${ingredient.quantity} ${Units[ingredient.unitId]}`,
				notes: ingredient.notes,
			};
		});
	}

	getInstructionString() {
		return (this.currentRecipe.instructions || []).map(instruction => {
			return {
				defaultValue: instruction.description,
			};
		});
	}

	// eslint-disable-next-line
	handleFileUpload(event: any) {
		const files: FileList = event.target.files;
		const image: File = files[0];
		this.$store.dispatch("uploadRecipeImage", image);
	}

	handleSubmit(recipe: Recipe) {
		this.$v.$touch();

		if (this.$v.$invalid) {
			Vue.$toast.error("Please fix invalid fields.");
			return;
		}

		if (this.isEditMode) {
			// eslint-disable-next-line
			recipe.id = this.recipeId!;
			this.$store.dispatch("updateRecipe", recipe);
		} else {
			this.$store.dispatch("insertRecipe", recipe);
		}
		this.$v.$reset();
	}

	handleReset() {
		if (!this.isEditMode) {
			this.$store.dispatch("setRecipeById", this.recipeId);
		} else {
			this.$store.dispatch("createNewRecipe");
		}
		this.$v.$reset();
	}

	handleDeleteImage(fullName: string) {
		this.$store.dispatch("deleteImage", fullName);
	}

	beforeCreate() {
		this.$store.dispatch("setIsLoading", true);
	}

	created() {
		if (this.isEditMode) {
			this.$store.dispatch("setRecipeById", this.recipeId);
		} else {
			this.$store.dispatch("createNewRecipe");
		}
		this.$store.dispatch("setIsLoading", false);

		this.currentRecipe;
	}

	beforeDestroy() {
		this.$store.dispatch("destroyNewRecipe");
	}
}
</script>

<style scoped lang="scss">
.create-recipe {
	margin-top: 30px;
	background-color: #e9ecef;
	border-radius: 20px;
	padding: 30px 5%;

	.button-wrapper {
		.btn {
			width: 90px;
			height: 50px;
			font-size: 1.2em;
		}
	}
}
</style>
