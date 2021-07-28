// CREDIT evarevirus's https://bootsnipp.com/snippets/aMnz0
<template>
	<section class="login">
		<div class="wrapper fadeInDown">
			<div id="formContent">
				<!-- Tabs Titles -->

				<!-- Icon -->
				<div class="fadeIn first icon-wrapper mb-0">
					<b-icon class="login-icon" icon="person"></b-icon>
					<!-- <img src="http://danielzawadzki.com/codepen/01/icon.svg" id="icon" alt="User Icon" /> -->
				</div>

				<!-- Login Form -->
				<form @submit.prevent="sendMagicToken" method="POST">
					<b-form-group :class="{ 'form-group--error': $v.email.$error }">
						<input
							type="text"
							id="email"
							class="fadeIn second"
							name="email"
							placeholder="email@gmail.com"
							v-model.trim="$v.email.$model"
						/>
						<div class="error" v-if="!$v.email.required && $v.email.$error">
							Email is required
						</div>
						<div class="error" v-if="!$v.email.email && $v.email.$error">
							Must be in email format (email@domain.com)
						</div>
					</b-form-group>
					<input
						type="submit"
						class="fadeIn fourth"
						value="Log In"
						:disabled="$v.$invalid"
					/>
				</form>

				<div id="formFooter"></div>
			</div>
		</div>
	</section>
</template>

<script lang="ts">
import { Component, Vue } from "vue-property-decorator";
import { Validate } from "vuelidate-property-decorators";
import { validationMixin } from "vuelidate";
import { required, email } from "vuelidate/lib/validators";
import { Magic } from "magic-sdk";
import { LoginRequest } from "@/models/AccountsModels";

const magic = new Magic(process.env.VUE_APP_MAGIC_KEY as string);

@Component({
	mixins: [validationMixin],
})
export default class Login extends Vue {
	get redirectRoute(): string | undefined {
		const route = this.$route.query.redirect;

		if (typeof route === "string") {
			return route;
		}
		return undefined;
	}

	async beforeCreate() {
		if (await magic.user.isLoggedIn()) {
			const idToken = await magic.user.getIdToken();
			const request: LoginRequest = {
				didToken: idToken,
				redirect: "/home",
			};
			this.$store.dispatch("getJwtToken", request);
		}
	}

	@Validate({ required, email })
	email = "";

	async sendMagicToken() {
		this.$v.$touch();

		if (this.$v.$invalid) {
			Vue.$toast.error("Please fix invalid fields.");
			return;
		}

		const redirectURI = this.redirectRoute
			? new URL(this.redirectRoute, window.location.origin).href
			: new URL("/authenticate", window.location.origin).href;

		console.log(await magic.apiKey);

		try {
			magic.auth.loginWithMagicLink({
				email: this.email,
				redirectURI: redirectURI,
			});
		} catch {
			Vue.$toast.error("Error logging in.");
		}
	}
}
</script>

<style lang="scss" scoped>
/* BASIC */
html {
	background-color: #56baed;
}

body {
	font-family: "Poppins", sans-serif;
	height: 100vh;
	letter-spacing: 1px;
}

a {
	color: #92badd;
	display: inline-block;
	text-decoration: none;
	font-weight: 400;
}

/* STRUCTURE */
.login {
	position: absolute;
	top: 0;
	bottom: 0;
	left: 0;
	right: 0;
	margin: auto;
}

.wrapper {
	display: flex;
	align-items: center;
	flex-direction: column;
	justify-content: center;
	width: 100%;
	min-height: 100%;
	padding: 20px;
}

form {
	margin-top: 10px;
}

#formContent {
	-webkit-border-radius: 10px 10px 10px 10px;
	border-radius: 10px 10px 10px 10px;
	background: #fff;
	width: 90%;
	max-width: 450px;
	position: relative;
	padding: 0;
	-webkit-box-shadow: 0 30px 60px 0 rgba(0, 0, 0, 0.3);
	box-shadow: 0 30px 60px 0 rgba(0, 0, 0, 0.3);
	text-align: center;
}

#formFooter {
	background-color: #f6f6f6;
	border-top: 1px solid #dce8f1;
	padding: 25px;
	text-align: center;
	-webkit-border-radius: 0 0 10px 10px;
	border-radius: 0 0 10px 10px;
}

/* TABS */

h2.inactive {
	color: #cccccc;
}

h2.active {
	color: #0d0d0d;
	border-bottom: 2px solid #5fbae9;
}

/* FORM TYPOGRAPHY*/

input[type="button"],
input[type="submit"],
input[type="reset"] {
	background-color: #56baed;
	border: none;
	color: white;
	padding: 15px 80px;
	text-align: center;
	text-decoration: none;
	display: inline-block;
	text-transform: uppercase;
	font-size: 13px;
	-webkit-box-shadow: 0 10px 30px 0 rgba(95, 186, 233, 0.4);
	box-shadow: 0 10px 30px 0 rgba(95, 186, 233, 0.4);
	-webkit-border-radius: 5px 5px 5px 5px;
	border-radius: 5px 5px 5px 5px;
	margin: 20px;
	-webkit-transition: all 0.3s ease-in-out;
	-moz-transition: all 0.3s ease-in-out;
	-ms-transition: all 0.3s ease-in-out;
	-o-transition: all 0.3s ease-in-out;
	transition: all 0.3s ease-in-out;
}

input[type="button"]:hover,
input[type="submit"]:hover,
input[type="reset"]:hover {
	background-color: #39ace7;
}

input[type="button"]:active,
input[type="submit"]:active,
input[type="reset"]:active {
	-moz-transform: scale(0.95);
	-webkit-transform: scale(0.95);
	-o-transform: scale(0.95);
	-ms-transform: scale(0.95);
	transform: scale(0.95);
}

input[type="text"] {
	background-color: #f6f6f6;
	border: none;
	color: #0d0d0d;
	padding: 15px 32px;
	text-align: center;
	text-decoration: none;
	display: inline-block;
	font-size: 16px;
	margin: 5px;
	width: 85%;
	border: 2px solid #f6f6f6;
	-webkit-transition: all 0.5s ease-in-out;
	-moz-transition: all 0.5s ease-in-out;
	-ms-transition: all 0.5s ease-in-out;
	-o-transition: all 0.5s ease-in-out;
	transition: all 0.5s ease-in-out;
	-webkit-border-radius: 5px 5px 5px 5px;
	border-radius: 5px 5px 5px 5px;
}

input[type="text"]:focus {
	background-color: #fff;
	border-bottom: 2px solid #5fbae9;
}

input[type="text"]:placeholder {
	color: #cccccc;
}

input[type="submit"]:disabled {
	background-color: #cccccc;
	color: #666666;
}

/* ANIMATIONS */

/* Simple CSS3 Fade-in-down Animation */
.fadeInDown {
	-webkit-animation-name: fadeInDown;
	animation-name: fadeInDown;
	-webkit-animation-duration: 1s;
	animation-duration: 1s;
	-webkit-animation-fill-mode: both;
	animation-fill-mode: both;
}

@-webkit-keyframes fadeInDown {
	0% {
		opacity: 0;
		-webkit-transform: translate3d(0, -100%, 0);
		transform: translate3d(0, -100%, 0);
	}
	100% {
		opacity: 1;
		-webkit-transform: none;
		transform: none;
	}
}

@keyframes fadeInDown {
	0% {
		opacity: 0;
		-webkit-transform: translate3d(0, -100%, 0);
		transform: translate3d(0, -100%, 0);
	}
	100% {
		opacity: 1;
		-webkit-transform: none;
		transform: none;
	}
}

/* Simple CSS3 Fade-in Animation */
@-webkit-keyframes fadeIn {
	from {
		opacity: 0;
	}
	to {
		opacity: 1;
	}
}
@-moz-keyframes fadeIn {
	from {
		opacity: 0;
	}
	to {
		opacity: 1;
	}
}
@keyframes fadeIn {
	from {
		opacity: 0;
	}
	to {
		opacity: 1;
	}
}

.fadeIn {
	opacity: 0;
	-webkit-animation: fadeIn ease-in 1;
	-moz-animation: fadeIn ease-in 1;
	animation: fadeIn ease-in 1;

	-webkit-animation-fill-mode: forwards;
	-moz-animation-fill-mode: forwards;
	animation-fill-mode: forwards;

	-webkit-animation-duration: 1s;
	-moz-animation-duration: 1s;
	animation-duration: 1s;
}

.fadeIn.first {
	-webkit-animation-delay: 0.4s;
	-moz-animation-delay: 0.4s;
	animation-delay: 0.4s;
}

.fadeIn.second {
	-webkit-animation-delay: 0.6s;
	-moz-animation-delay: 0.6s;
	animation-delay: 0.6s;
}

.fadeIn.third {
	-webkit-animation-delay: 0.8s;
	-moz-animation-delay: 0.8s;
	animation-delay: 0.8s;
}

.fadeIn.fourth {
	-webkit-animation-delay: 1s;
	-moz-animation-delay: 1s;
	animation-delay: 1s;
}

/* Simple CSS3 Fade-in Animation */
.underlineHover {
	color: #1589c4 !important;
}

.underlineHover:after {
	display: block;
	left: 0;
	bottom: -10px;
	width: 0;
	height: 2px;
	background-color: #56baed;
	content: "";
	transition: width 0.2s;
}

.underlineHover:hover {
	color: #0d0d0d !important;
}

.underlineHover:hover:after {
	width: 100%;
}

/* OTHERS */
.icon-wrapper {
	margin: 30px;
}

.login-icon {
	font-size: 50px;
}

small {
	display: block;
}
</style>
