<template lang="html">
	<section class="profile-update-password">
		<h4>Password</h4>
		<b-form @submit.prevent="handleSubmit">
			<b-form-row>
				<b-col md="12">
					<b-form-group
						label="Current Password"
						label-for="currentPassword"
						:class="{
							'form-group--error': $v.currentPassword.$error,
						}"
					>
						<b-form-input
							class="form__input"
							type="password"
							id="currentPassword"
							v-model.trim="$v.currentPassword.$model"
							required
						></b-form-input>
						<p
							class="error"
							v-if="!$v.currentPassword.required && $v.currentPassword.$error"
						>
							Field is required
						</p>
					</b-form-group>
				</b-col>
			</b-form-row>
			<b-form-row>
				<b-col md="12">
					<b-form-group
						label="New Password"
						label-for="newPassword"
						:class="{
							'form-group--error': $v.newPassword.$error,
						}"
					>
						<b-form-input
							class="form__input"
							type="password"
							id="newPassword"
							v-model.trim="$v.newPassword.$model"
							required
						></b-form-input>
						<p v-if="$v.newPassword.$error" class="error">
							<span v-if="!$v.newPassword.required"
								>This field is required.</span
							>
							<span v-else-if="!$v.newPassword.strongPassword"
								>Stronger password required.</span
							>
						</p>
					</b-form-group>
				</b-col>
			</b-form-row>
			<b-form-row>
				<b-col md="12">
					<b-form-group
						label="Confirm Password"
						label-for="confirmPassword"
						:class="{
							'form-group--error': $v.newPasswordConfirmation.$error,
						}"
					>
						<b-form-input
							class="form__input"
							type="password"
							id="confirmPassword"
							v-model.trim="$v.newPasswordConfirmation.$model"
							required
						></b-form-input>
						<p v-if="$v.newPasswordConfirmation.$error" class="error">
							<span v-if="!$v.newPasswordConfirmation.required"
								>This field is required.</span
							>
							<span v-else-if="!$v.newPasswordConfirmation.sameAs"
								>New passwords must match.</span
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
import { required, sameAs } from "vuelidate/lib/validators";
import { Component, Vue } from "vue-property-decorator";
import { validPassword } from "@/mixins/passwordValidation";
import { UpdatePasswordCommand } from "@/models/AccountsModels";

@Component({
	mixins: [validationMixin],
})
export default class ProfileUpdatePassword extends Vue {
	@Validate({ required })
	currentPassword = "";

	@Validate({
		required,
		strongPassword: validPassword,
	})
	newPassword = "";

	@Validate({ required, sameAs: sameAs("newPassword") })
	newPasswordConfirmation = "";

	handleSubmit() {
		this.$v.$touch();

		if (this.$v.$invalid) {
			Vue.$toast.error("Please fix invalid fields.");
			return;
		}

		const payload: UpdatePasswordCommand = {
			currentPassword: this.currentPassword,
			newPassword: this.newPassword,
			newPasswordConfirmation: this.newPasswordConfirmation,
		};

		this.$store.dispatch("updatePassword", payload);
	}
}
</script>

<style scoped lang="scss">
.profile-update-password {
	.error {
		color: red;
	}

	.form-group--error {
		input {
			border-color: red;
		}
	}

	*:focus {
		outline: none;
		border-color: none;
		-webkit-box-shadow: none;
		box-shadow: none;
	}
}
</style>
