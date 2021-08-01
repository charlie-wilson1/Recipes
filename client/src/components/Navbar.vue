<template>
	<section class="navbar-section">
		<b-navbar toggleable="lg" type="dark" variant="primary" fixed="true">
			<b-navbar-brand href="/">Recipes</b-navbar-brand>
			<b-navbar-toggle target="nav-collapse"></b-navbar-toggle>
			<b-collapse id="nav-collapse" is-nav>
				<b-navbar-nav v-if="isLoggedIn">
					<b-nav-item href="/create">Create</b-nav-item>
				</b-navbar-nav>
				<b-navbar-nav v-if="isAdmin">
					<b-nav-item-dropdown text="Admin" right>
						<b-dropdown-item :to="{ name: 'manageUsers' }"
							>Manage Users</b-dropdown-item
						>
					</b-nav-item-dropdown>
				</b-navbar-nav>
				<b-navbar-nav class="ml-auto" v-if="isLoggedIn">
					<b-nav-item-dropdown :text="username" right>
						<b-dropdown-item href="/profile">Profile</b-dropdown-item>
						<b-dropdown-item href="/logout">Logout</b-dropdown-item>
					</b-nav-item-dropdown>
				</b-navbar-nav>
				<b-navbar-nav class="ml-auto" v-else>
					<b-nav-item href="/login">Login</b-nav-item>
				</b-navbar-nav>
			</b-collapse>
		</b-navbar>
	</section>
</template>

<script lang="ts">
import { Component, Vue } from "vue-property-decorator";

@Component({})
export default class Navbar extends Vue {
	get isLoggedIn(): boolean {
		return this.$store.state.isLoggedIn;
	}

	get isAdmin(): boolean {
		if (!this.$store.state.isLoggedIn) {
			return false;
		}

		return this.$store.getters.isAdmin;
	}

	get username(): string {
		return `Hello ${this.$store.state.username}`;
	}
}
</script>

<style lang="scss" scoped>
.navbar-section {
	margin-bottom: 2em;
}
</style>
