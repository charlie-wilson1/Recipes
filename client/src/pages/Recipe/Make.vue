<template lang="html">
	<section class="loader vh-100 text-center d-flex" v-if="isLoading">
		<b-spinner label="Loading..."></b-spinner>
	</section>
	<section class="make" v-else>
		<b-container>
			<h1>{{ recipe.name }}</h1>
			<b-row>
				<b-col md="9">
					<div class="instructions-container">
						<OrderedListGroup
							:values="instructions"
							:is-indexed="true"
							style="font-size: 1.8em;"
						/>
					</div>
				</b-col>
				<b-col>
					<OrderedListGroup
						:values="ingredients"
						:is-indexed="false"
						style="font-size: 1.8em;"
					/>
				</b-col>
			</b-row>
		</b-container>
	</section>
</template>

<script lang="ts">
import { Component, Vue } from "vue-property-decorator";
import { Ingredient, Instruction, Recipe } from "@/models/RecipeModels";
import OrderedListGroup from "@/components/shared/OrderedListGroup.vue";

@Component({
	components: {
		OrderedListGroup,
	},
})
export default class Make extends Vue {
	get recipe(): Recipe {
		return this.$store.getters.selectedRecipe;
	}

	get instructions() {
		const sorted = this.sortOrderableObjectArray(
			this.recipe?.instructions
		) as Array<Instruction>;
		return sorted.map(x => x.description);
	}

	get ingredients() {
		const sorted = this.sortOrderableObjectArray(
			this.recipe?.ingredients
		) as Array<Ingredient>;
		return sorted.map(x => x.name);
	}

	get isLoading() {
		return this.$store.getters.isLoading;
	}

	sortOrderableObjectArray(
		orderableArray: Array<Instruction> | Array<Ingredient> | undefined
	): Array<Instruction> | Array<Ingredient> {
		if (!orderableArray) {
			return [];
		}

		return orderableArray.sort(
			(a: Instruction | Ingredient, b: Instruction | Ingredient) => {
				if (a.orderNumber > b.orderNumber) {
					return 1;
				} else {
					return -1;
				}
			}
		);
	}

	async beforeCreate() {
		await this.$store.dispatch("setIsLoading", true);

		const id = this.$route.params.recipe_id;
		await this.$store.dispatch("getRecipeById", id);

		await this.$store.dispatch("setIsLoading", false);
	}

	beforeDestroy() {
		this.$store.dispatch("destroyNewRecipe");
	}
}
</script>

<style scoped lang="scss">
.make {
	margin-top: 30px;

	h1 {
		font-size: 4em;
	}

	.instructions-container {
		overflow-x: none;
		overflow-y: scroll;
		word-wrap: break-word;
		height: 80vh;
	}
}
</style>
