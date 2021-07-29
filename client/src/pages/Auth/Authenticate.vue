<template
	><section class="loader vh-100 text-center d-flex">
		<b-spinner label="Loading..."></b-spinner></section
></template>

<script lang="ts">
import { Component, Vue } from "vue-property-decorator";
import { Magic } from "magic-sdk";
import { LoginRequest } from "@/models/AccountsModels";

const magic = new Magic(process.env.VUE_APP_MAGIC_KEY as string);

@Component({})
export default class Authenticate extends Vue {
	get redirectRoute(): string | undefined {
		const route = this.$route.query.redirect;

		if (typeof route === "string") {
			return route;
		}
		return undefined;
	}

	async beforeCreate() {
		this.$store.dispatch("setIsLoading", true);

		const didToken = await magic.auth.loginWithCredential(
			window.location.search
		);

		const payload: LoginRequest = {
			didToken,
			redirect: this.redirectRoute ?? "/home",
		};

		this.$store.dispatch("getJwtToken", payload);
	}
}
</script>
