<template lang="html">
	<section class="recipe-container">
		<b-container>
			<b-row class="recipe-main">
				<b-col cols="4" class="list-col" ref="recipeListContainer">
					<b-form>
						<b-form-group id="search-recipes-group" class="search-recipes">
							<b-form-input
								id="search-recipes-input"
								v-model.trim="query"
								type="text"
								placeholder="Search Recipes"
							></b-form-input>
						</b-form-group>
					</b-form>
					<div class="recipe-list">
						<div v-if="listIsLoading" class="loader vh-100 text-center d-flex">
							<b-spinner label="Loading..."></b-spinner>
						</div>
						<div v-else>
							<b-list-group flush>
								<b-list-group-item
									class="recipe-list-item"
									v-for="(recipe, index) in recipes"
									:key="recipe.id"
									:active="recipe.id === selectedRecipe.id"
									@click="setSelectedRecipe(index)"
								>
									{{ recipe.name }}
								</b-list-group-item>
							</b-list-group>
						</div>
					</div>
				</b-col>
				<b-col cols="8" class="recipe-description-wrapper">
					<RecipeDescription :recipe="selectedRecipe" />
				</b-col>
			</b-row>
			<b-row class="recipe-buttons">
				<b-col cols="4" class="pagination">
					<b-pagination
						v-model="currentPage"
						:current-page="currentPage"
						:total-rows="totalRecipes"
						:per-page="perPage"
						aria-controls="recipesList"
						hide-goto-end-buttons
						limit="3"
						size="lg"
						class="mt-4"
					>
					</b-pagination>
				</b-col>
				<b-col cols="8" class="text-center">
					<b-button
						:to="{
							name: 'make',
							params: { recipe_id: getRecipeId(selectedRecipe.id) },
						}"
						absolute
						variant="primary"
					>
						Make
					</b-button>
					<b-button
						:to="{
							name: 'edit',
							params: { recipe_id: getRecipeId(selectedRecipe.id) },
						}"
						variant="warning"
					>
						Edit
					</b-button>
					<b-button variant="success">Buy</b-button>
				</b-col>
			</b-row>
		</b-container>
	</section>
</template>

<script lang="ts">
import { Component, Vue, Watch } from "vue-property-decorator";
import { Debounce } from "vue-debounce-decorator";
import RecipeDescription from "./RecipeDescription.vue";
import { GetAllRecipesQuery, Recipe } from "../../models/RecipeModels";

@Component({
	components: {
		RecipeDescription,
	},
})
export default class RecipeContainer extends Vue {
	public currentPage = 1;
	public perPage = 9;
	public query = "";

	listIsLoading = false;

	get totalRecipes(): number {
		return this.$store.getters.recipeCount;
	}

	get startNumber(): number {
		return this.currentPage === 1
			? this.currentPage
			: (this.currentPage - 1) * this.perPage + 1;
	}

	get recipes(): Array<Recipe> {
		return this.$store.getters.currentRecipeList;
	}

	get selectedRecipe(): Recipe {
		return this.$store.getters.selectedRecipe;
	}

	@Watch("query")
	@Debounce(500)
	async search() {
		this.currentPage = 1;
		await this.updateRecipeList();
	}

	@Watch("currentPage")
	async updateRecipeList() {
		const query: GetAllRecipesQuery = {
			resultsPerPage: this.perPage,
			pageNumber: this.currentPage,
			searchQuery: this.query,
		};

		this.listIsLoading = true;
		await this.$store.dispatch("loadRecipeList", query);
		this.listIsLoading = false;
	}

	setSelectedRecipe(index: number) {
		const actualIndex = this.startNumber + index - 1;

		if (this.recipes.length - 1 < actualIndex) {
			return;
		}

		this.$store.dispatch("setSelectedRecipe", actualIndex);
	}

	getRecipeId(recipeId: string): string {
		return recipeId.substring(recipeId.indexOf("/") + 1);
	}
}
</script>

<style scoped lang="scss">
@import "public/css/values.scss";

.recipe-container {
	padding: $recipe-padding 0;
	margin: $recipe-margin;
	height: $recipe-container-height;
	min-height: 860px;
	border: 2px solid lightgrey;
	overflow: hidden;
	border-radius: 10px;

	.list-col {
		padding-left: 0;
	}

	.search-recipes {
		margin-left: 15px;
		margin-right: 15px;
	}

	.recipe-list {
		height: 93%;
		overflow-x: hidden;
		overflow-y: scroll;

		&::-webkit-scrollbar {
			width: 5px;
		}

		&::-webkit-scrollbar-track {
			background: #f1f1f1;
		}

		&::-webkit-scrollbar-thumb {
			background: #888;
			outline: 1px solid slategrey;
		}

		&::-webkit-scrollbar-thumb:hover {
			background: #555;
		}
	}

	.recipe-list-item {
		align-items: center;
		line-height: 50px;
		width: 104.55%;
		font-size: 1.5em;
		overflow: hidden;
		text-overflow: ellipsis;
		word-wrap: break-word;
		white-space: nowrap;
		display: block;
		border-left: none;
		border-right: none;
		border-radius: 0;

		&:first-child {
			border-top: none;
		}

		&:hover {
			cursor: pointer;
		}
	}

	.list-group-item {
		border-bottom: 2px solid lightgrey;
	}

	.list-group-item:last-of-type {
		border-bottom: none;
	}

	.recipe-description-wrapper::before {
		position: absolute;
		left: 0;
		right: -20px;
		top: -20px;
		bottom: 0;
		content: " ";
		display: block;
		border-left: 2px solid lightgrey;
		border-radius: 0 0 0 5px;
		-webkit-box-shadow: 0px 0px 23px 5px rgba(0, 0, 0, 0.1);
		box-shadow: 0px 0px 23px 5px rgba(0, 0, 0, 0.1);
	}

	.recipe-buttons {
		min-height: $recipe-button-height;
		margin-top: 30px;

		.btn {
			margin: 10px;
			padding: 0.75em 1.5em;
		}

		.pagination {
			margin-top: -18px;
			margin-right: auto;
			margin-left: auto;
		}
	}
}
</style>
