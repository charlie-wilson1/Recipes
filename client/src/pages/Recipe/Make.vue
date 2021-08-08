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
			<b-row v-if="youTubeUrls">
				<b-carousel :model="slide" :interval="0" indicators controls>
					<b-carousel-slide
						v-for="(url, index) in youTubeUrls"
						:key="index"
						img-blank
						fluid-grow
					>
						<template #img>
							<b-embed
								class="youtube-player"
								:src="`https://www.youtube.com/embed/${getYouTubeId(url)}`"
								ref="youtube"
								width="100%"
								height="650px"
								aspect="16by9"
							></b-embed>
						</template> </b-carousel-slide
				></b-carousel>
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
		return this.$store.state.RecipeModule.selectedRecipe;
	}

	get instructions() {
		const sorted = this.sortOrderableObjectArray(
			this.recipe?.instructions
		) as Instruction[];
		return sorted.map(x => x.description);
	}

	get ingredients() {
		const sorted = this.sortOrderableObjectArray(
			this.recipe?.ingredients
		) as Ingredient[];
		return sorted.map(x => x.name);
	}

	get isLoading() {
		return this.$store.state.isLoading;
	}

	sortOrderableObjectArray(
		orderableArray: Instruction[] | Ingredient[] | undefined
	): Instruction[] | Ingredient[] {
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

	slide = 0;

	get youTubeUrls(): string[] | undefined {
		return this.recipe.youTubeUrls;
	}

	getYouTubeId(url: string): string {
		const uri = new URL(url);
		if (uri && uri.searchParams.get("v") !== null) {
			// eslint-disable-next-line  @typescript-eslint/no-non-null-assertion
			return uri.searchParams.get("v")!.toString();
		}
		return "";
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
@keyframes fade-in-up {
	0% {
		opacity: 0;
	}
	100% {
		transform: translateY(0);
		opacity: 1;
	}
}

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

	.carousel {
		width: 100%;
		margin-top: 50px;

		.carousel-inner {
			margin-left: auto;
			margin-right: auto;
		}
	}
}
</style>
