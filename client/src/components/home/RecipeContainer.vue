<template lang="html">
	<section>
		<b-container class="recipe-container">
			<b-row class="recipe-main mb-5">
				<b-col cols="4" class="list-col flex-column" ref="recipeListContainer">
					<b-container>
						<b-row>
							<b-col class="p-0">
								<b-form>
									<b-form-group id="search-recipes-group">
										<b-form-input
											id="search-recipes-input"
											v-model.trim="query"
											type="text"
											placeholder="Search Recipes"
										></b-form-input>
									</b-form-group>
								</b-form>
							</b-col>
						</b-row>
						<b-row class="list-row">
							<b-col>
								<div
									v-if="listIsLoading"
									class="loader vh-100 text-center d-flex"
								>
									<b-spinner label="Loading..."></b-spinner>
								</div>
								<b-list-group
									class="recipe-list-group flex-container h-100 pb-5"
									v-else
								>
									<b-list-group-item
										class="recipe-list-item row flex-grow-1"
										v-for="recipe in recipes"
										:key="recipe.id"
										:active="recipe.id === selectedRecipe.id"
										@click="setSelectedRecipe(recipe)"
									>
										{{ recipe.name }}
									</b-list-group-item>
								</b-list-group>
							</b-col>
						</b-row>
					</b-container>
				</b-col>
				<b-col cols="8" class="recipe-description-wrapper">
					<RecipeDescription :recipe="selectedRecipe" />
				</b-col>
			</b-row>
			<b-row class="recipe-buttons align-middle pb-4">
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
				<b-col cols="8" class="text-center description-buttons">
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
	public perPage = 10;
	public query = "";

	listIsLoading = false;

	get totalRecipes(): number {
		return this.$store.state.RecipeModule.recipeCount;
	}

	get startNumber(): number {
		return this.currentPage === 1
			? this.currentPage
			: (this.currentPage - 1) * this.perPage + 1;
	}

	get recipes(): Array<Recipe> {
		return this.$store.state.RecipeModule.currentRecipeList;
	}

	get selectedRecipe(): Recipe {
		return this.$store.state.RecipeModule.selectedRecipe;
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

	setSelectedRecipe(recipe: Recipe) {
		this.$store.dispatch("setSelectedRecipe", recipe);
	}

	getRecipeId(recipeId: string): string {
		return recipeId.substring(recipeId.indexOf("/") + 1);
	}
}
</script>

<style scoped lang="scss">
.recipe-container {
	padding: 10px;
	padding-bottom: 0;
	height: 90vh;

	.row {
		flex: none;
	}

	.recipe-main {
		height: 92%;
		margin-bottom: 2%;
		overflow: hidden;

		.container {
			height: 100%;

			.list-row {
				height: 100%;
			}

			.recipe-list-group {
				width: 100%;
				border-radius: 0;
				overflow-y: visible;

				.recipe-list-item {
					font-size: 1.2em;
					overflow-x: hidden;
					text-overflow: ellipsis;
					word-wrap: break-word;
					white-space: nowrap;
					border-right: none;
					border-left: none;
					display: flex;
					flex-direction: row;
					align-items: center;

					&:hover {
						cursor: pointer;
					}

					&:first-child {
						border-top: none;
					}

					&:last-child {
						border-bottom: none;
					}

					&:hover {
						cursor: pointer;
					}
				}

				.list-group-item:last-of-type {
					border-bottom: none;
				}
			}
		}

		.recipe-description-wrapper {
			height: 100%;
		}
	}

	.recipe-buttons {
		min-height: 30px;

		.btn {
			margin: auto;
			padding: 0.75em 1.5em;
		}

		.pagination {
			margin-right: auto;
			margin-left: auto;
			margin-bottom: auto !important;
			margin-top: auto !important;
		}

		.description-buttons {
			margin-bottom: auto;
			margin-top: auto;

			.btn {
				margin: 0 10px;
			}
		}
	}
}
</style>
