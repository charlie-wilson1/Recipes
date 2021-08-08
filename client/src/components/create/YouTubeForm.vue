<template>
	<section class="youtube-form">
		<b-form-group
			:class="{
				'form-group--error': $v.youtubeUrl.$error,
			}"
		>
			<label for="youtubeUrls">YouTubeUrls</label>
			<div>
				<b-input-group>
					<b-form-textarea
						type="text"
						v-model="$v.youtubeUrl.$model"
						id="youtubeUrls"
						placeholder="https://youtube.com/watch?v=5rlsUfQTRzs"
					></b-form-textarea>
					<b-input-group-append>
						<b-button
							@click="addYouTubeUrl"
							:disabled="$v.$invalid"
							variant="secondary"
							>Add
						</b-button>
					</b-input-group-append>
				</b-input-group>
			</div>
		</b-form-group>
	</section>
</template>

<script lang="ts">
import { Component, Vue } from "vue-property-decorator";
import { Validate } from "vuelidate-property-decorators";
import { validationMixin } from "vuelidate";
import { required } from "vuelidate/lib/validators";

@Component({
	mixins: [validationMixin],
})
export default class YouTubeForm extends Vue {
	@Validate({ required })
	youtubeUrl = "";

	addYouTubeUrl() {
		this.$v.$touch();

		if (this.$v.$invalid) {
			Vue.$toast.error("Please fill out required fields.");
			return;
		}

		this.$store.dispatch("insertYouTubeUrl", this.youtubeUrl);
		this.$v.$reset();
		this.youtubeUrl = "";
	}
}
</script>

<style lang="scss" scoped>
.youtubes-form {
}
</style>
