<template lang="html">
	<section class="profile container">
		<h1>Profile</h1>
		<b-form @submit.prevent="handleSubmit">
			<b-form-row>
				<b-col md="4" offset="4">
					<b-form-group
						label="Username"
						label-for="username"
						:class="{
							'form-group--error': $v.username.$error,
						}"
					>
						<b-form-input
							class="form__input"
							type="text"
							id="username"
							v-model.trim="$v.username.$model"
							required
						></b-form-input>
						<p class="error" v-if="!$v.username.required && $v.username.$error">
							Field is required
						</p>
					</b-form-group>
				</b-col>
			</b-form-row>
			<b-form-group class="text-center button-wrapper">
				<b-button type="submit" variant="primary mr-2" :disabled="$v.$invalid"
					>Submit
				</b-button>
				<b-button type="reset" variant="secondary">Clear </b-button>
			</b-form-group>
		</b-form>
	</section>
</template>

<script lang="ts">
import { Validate } from "vuelidate-property-decorators";
import { validationMixin } from "vuelidate";
import { required } from "vuelidate/lib/validators";
import { Component, Vue } from "vue-property-decorator";
import { UpdateUsernameRequest } from "@/models/AccountsModels";

@Component({
	mixins: [validationMixin],
})
export default class Profile extends Vue {
	get currentUsername(): string {
		return this.$store.state.username;
	}

	@Validate({ required })
	username = this.currentUsername ?? "";

	handleSubmit() {
		this.$v.$touch();

		if (this.$v.$invalid) {
			Vue.$toast.error("Please fix invalid fields.");
			return;
		}

		const payload: UpdateUsernameRequest = {
			username: this.username,
			email: this.$store.state.email,
		};

		this.$store.dispatch("updateUsername", payload);
	}
}
</script>

<style scoped lang="scss">
.profile {
	&.container {
		margin-top: 2em;
	}

	h1 {
		text-align: center;
	}

	.form-control:focus {
		border: 1px solid #ced4da;
	}
}
</style>
