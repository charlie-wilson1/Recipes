<template lang="html">
	<section class="confirm-reset-password container">
		<h1>Reset Password</h1>
		<b-form @submit.prevent="handleSubmit">
			<PasswordInput
				:password.sync="$v.newPassword.$model"
				label="New Password"
				:errors="$v.newPassword"
			/>
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
import PasswordInput from "@/components/Auth/PasswordInput.vue";
import { ConfirmResetPasswordCommand } from "@/models/AccountsModels";
import { redirect } from "@/mixins/routingUtils";

@Component({
	components: { PasswordInput },
	mixins: [validationMixin],
})
export default class ConfirmResetPassword extends Vue {
	created() {
		this.email;
		this.redirectRoute;
	}

	get redirectRoute(): string | undefined {
		const queryRedirectRoute = this.$route.query.redirectRoute;

		if (typeof queryRedirectRoute === "string") {
			return queryRedirectRoute;
		}

		return undefined;
	}

	get email(): string | undefined {
		const queryEmail = this.$route.query.email;

		if (typeof queryEmail === "string") {
			return queryEmail;
		}

		redirect("Invalid link. Send request to admin for registration email");
		return undefined;
	}

	get resetToken(): string | undefined {
		const token = this.$route.query.token;

		if (typeof token === "string") {
			return token;
		}

		redirect("Invalid link. Send request to admin for registration email");
		return undefined;
	}

	@Validate({
		required,
		strongPassword: validPassword,
	})
	newPassword = "";

	@Validate({ required, sameAs: sameAs("newPassword") })
	newPasswordConfirmation = "";

	handleSubmit() {
		if (!this.email || !this.resetToken) {
			redirect("Invalid link. Send request to admin for registration email");
		}

		this.$v.$touch();

		if (this.$v.$invalid) {
			Vue.$toast.error("Please fix invalid fields.");
			return;
		}

		const command: ConfirmResetPasswordCommand = {
			newPassword: this.newPassword,
			newPasswordConfirmation: this.newPasswordConfirmation,
			// eslint-disable-next-line
			email: this.email!,
			// eslint-disable-next-line
			resetToken: this.resetToken!,
			redirect: this.redirectRoute,
		};

		this.$store.dispatch("confirmResetPassword", command);
	}
}
</script>

<style scoped lang="scss">
.confirm-reset-password {
}
</style>
