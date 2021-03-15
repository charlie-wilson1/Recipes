<template lang="html">
	<section class="profile-update-user">
		<h4>Update User</h4>
		<b-form @submit.prevent="handleSubmit">
			<b-form-row>
				<b-col md="12">
					<b-form-group
						label="Username"
						label-for="username"
						:class="{
							'form-group--error': $v.username.$error,
						}"
					>
						<b-form-input
							class="form__input"
							type="username"
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
			<b-form-row>
				<b-col md="12">
					<b-form-group
						label="Email"
						label-for="email"
						:class="{
							'form-group--error': $v.email.$error,
						}"
					>
						<b-form-input
							class="form__input"
							type="email"
							id="email"
							v-model.trim="$v.email.$model"
							required
						></b-form-input>
						<p v-if="$v.email.$error" class="error">
							<span v-if="!$v.email.required">This field is required.</span>
							<span v-else-if="!$v.email.email"
								>Must be a valid email address.</span
							>
						</p>
					</b-form-group>
				</b-col>
			</b-form-row>
			<b-form-group class="text-center button-wrapper">
				<b-button type="submit" variant="success mr-2" :disabled="$v.$invalid"
					>Submit
				</b-button>
				<b-button type="reset" variant="primary">Clear </b-button>
			</b-form-group>
		</b-form>
	</section>
</template>

<script lang="ts">
import { Validate } from "vuelidate-property-decorators";
import { validationMixin } from "vuelidate";
import { required, email } from "vuelidate/lib/validators";
import { Component, Vue } from "vue-property-decorator";
import { UpdateCurrentUserCommand } from "@/models/AccountsModels";

@Component({
	mixins: [validationMixin],
})
export default class ProfileUpdateUser extends Vue {
	get currentUsername(): string {
		return this.$store.getters.username;
	}

	@Validate({ required })
	username = this.currentUsername;

	@Validate({ required, email })
	email = "";

	handleSubmit() {
		this.$v.$touch();

		if (this.$v.$invalid) {
			Vue.$toast.error("Please fix invalid fields.");
			return;
		}

		const payload: UpdateCurrentUserCommand = {
			username: this.username,
			email: this.email,
		};

		this.$store.dispatch("updateUser", payload);
	}
}
</script>

<style scoped lang="scss">
.profile-update-user {
}
</style>
