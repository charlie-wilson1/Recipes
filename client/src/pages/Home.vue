<template>
	<section class="loader vh-100 text-center d-flex" v-if="isLoading">
		<b-spinner label="Loading..."></b-spinner>
	</section>
	<section v-else-if="userHasRecipes" class="recipe-home container">
		<RecipeContainer />
	</section>
	<section v-else class="no-recipes">
		<h1>No Recipes!</h1>
		<p>Could not find any of your recipes.</p>
		<p>Try Creating one by clicking the button below!</p>
		<b-button
			:to="{ name: 'create' }"
			absolute
			variant="success"
			class="create-btn"
		>
			Create
		</b-button>
	</section>
</template>

<script lang="ts">
import { GetAllRecipesQuery } from "@/models/RecipeModels";
import { Component, Vue } from "vue-property-decorator";
import RecipeContainer from "../components/home/RecipeContainer.vue";

@Component({
	components: {
		RecipeContainer,
	},
})
export default class Home extends Vue {
	get userHasRecipes(): boolean {
		return (this.$store.state.RecipeModule.recipeCount || 0) > 0;
	}

	async beforeCreate() {
		const query: GetAllRecipesQuery = {
			resultsPerPage: 10,
			pageNumber: 1,
			searchQuery: undefined,
		};

		await this.$store.dispatch("setIsLoading", true);
		await this.$store.dispatch("loadRecipeList", query);
		await this.$store.dispatch("setIsLoading", false);
	}

	get isLoading() {
		return this.$store.state.isLoading;
	}
}
</script>

<style lang="scss" scoped>
.no-recipes {
	margin: auto;
	width: 50%;
	padding: 10px;
	justify-content: center;
	height: 80vh;
	line-height: 1em;
	align-items: center;
	display: flex;
	flex-direction: column;

	h1 {
		padding-bottom: 0.5em;
	}

	.create-btn {
		margin-top: 0.5em;
	}
}
</style>
