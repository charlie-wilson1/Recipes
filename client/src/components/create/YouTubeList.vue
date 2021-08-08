<template lang="html">
	<section class="create-list">
		<b-list-group>
			<b-list-group-item v-for="(url, index) in youtubeUrls" :key="index">
				<b-row>
					<b-col cols="10">
						<b-embed
							type="iframe"
							aspect="16by9"
							:src="`https://www.youtube.com/embed/${getYouTubeId(url)}`"
						></b-embed>
					</b-col>
					<b-col>
						<div class="delete-wrapper">
							<b-icon
								icon="trash"
								scale="2"
								@click="handleDelete(url)"
							></b-icon>
						</div>
					</b-col>
				</b-row>
			</b-list-group-item>
		</b-list-group>
	</section>
</template>

<script lang="ts">
import { Component, Vue } from "vue-property-decorator";

@Component({})
export default class YouTubeList extends Vue {
	get youtubeUrls(): string[] {
		return this.$store.state.NewRecipeModule.recipe.youTubeUrls;
	}

	handleDelete(url: string) {
		this.$store.dispatch("removeYouTubeUrl", url);
	}

	getYouTubeId(url: string): string {
		const uri = new URL(url);
		if (uri && uri.searchParams.get("v") !== null) {
			// eslint-disable-next-line  @typescript-eslint/no-non-null-assertion
			return uri.searchParams.get("v")!.toString();
		}
		return "";
	}
}
</script>

<style scoped lang="scss">
.create-list {
	padding-top: 0px;

	.list-group {
		margin-left: 35px;
		margin-top: 5px;
		width: 90%;
	}

	.list-group-item {
		border: none;
		padding-left: 0;
		padding-right: 0;
		background-color: transparent;
	}

	.delete-wrapper {
		display: flex;
		height: 100%;

		.b-icon {
			height: 100%;
			vertical-align: middle;
		}
	}

	svg {
		cursor: pointer;
	}
}
</style>
