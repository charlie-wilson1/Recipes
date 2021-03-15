<template lang="html">
	<section class="password-input">
		<b-form-row>
			<b-col md="12">
				<b-form-group :label="label" label-for="password">
					<b-input-group class="has-feedback-right">
						<b-form-input
							class="form__input"
							:type="showPassword ? 'text' : 'password'"
							v-model.trim="passwordValue"
							:class="{
								'form-group--error': errors.$error,
							}"
							required
						></b-form-input>
						<span class="form-control form-control-feedback pr-0 mr-0">
							<b-icon
								:icon="showPassword ? 'eye' : 'eye-slash'"
								@click="toggleShowPassword"
							></b-icon>
						</span>
						<!-- <small @click="toggleShowPassword" class="text-muted"
							>Show password</small
						> -->
					</b-input-group>
					<p v-if="errors.$error" class="error">
						<span v-if="!errors.required">This field is required.</span>
						<span v-else-if="!errors.strongPassword"
							>Stronger password required.</span
						>
					</p>
				</b-form-group>
			</b-col>
		</b-form-row>
	</section>
</template>

<script lang="ts">
import { Component, Prop, PropSync, Vue } from "vue-property-decorator";

@Component({})
export default class PasswordInput extends Vue {
	@PropSync("password", {
		type: String,
		required: true,
		default: "",
	})
	passwordValue!: string;

	@Prop({ default: "Password", required: false })
	label!: string;

	@Prop({ default: {}, required: true })
	errors!: {};

	showPassword = false;

	toggleShowPassword() {
		this.showPassword = !this.showPassword;
	}
}
</script>

<style scoped lang="scss">
.password-input {
	.form__input {
		width: 94%;
	}

	.form-control-feedback {
		border-left: none;
	}
}
</style>
