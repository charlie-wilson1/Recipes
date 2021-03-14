<template lang="html">
	<section class="manage-users container">
		<h1>Manage Users</h1>
		<b-list-group class="users">
			<b-list-group-item v-for="user in users" :key="user.id">
				<span>{{ user.username }}</span>
				<span class="float-right"></span>
			</b-list-group-item>
		</b-list-group>
	</section>
</template>

<script lang="ts">
import { User } from "@/models/AdministratorModels";
import { Component, Vue } from "vue-property-decorator";

@Component({})
export default class ManageUsers extends Vue {
	get users(): Array<User> {
		return this.$store.getters.users || [];
	}

	beforeCreate() {
		this.$store.dispatch("getUsers");
	}
}
</script>

<style scoped lang="scss">
.manage-users {
}
</style>
