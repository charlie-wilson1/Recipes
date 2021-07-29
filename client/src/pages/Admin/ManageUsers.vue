<template lang="html">
	<section class="manage-users container">
		<b-row class="d-flex justify-content-between">
			<h1>Manage Users</h1>
			<span
				><b-button class="mb-2 mr-2" variant="success" v-b-modal.add-users-modal
					><b-icon icon="plus" class="mr-2"></b-icon>Add Users</b-button
				></span
			>
		</b-row>
		<b-card>
			<b-tabs pills card vertical class="users">
				<b-tab
					v-for="user in users"
					:key="user.email"
					:title="user.username || user.email"
					:active="user.email === selectedEmail"
					:disabled="user.username === currentUsername"
					@click="setSelectedUser(user)"
					><b-card-text>
						<div class="d-flex justify-content-between">
							<h3>{{ user.username }}</h3>
							<span class="card-icons">
								<b-button
									class="mb-2 mr-2"
									variant="warning"
									v-b-modal.manage-user-roles-modal
									><b-icon icon="pencil" class="mr-2"></b-icon>Edit
									Roles</b-button
								>
								<b-button
									class="mb-2"
									variant="danger"
									v-b-modal.delete-user-modal
									><b-icon icon="trash" class="mr-2"></b-icon>Delete</b-button
								></span
							>
						</div>
						<b-list-group flush>
							<b-list-group-item
								class="role-list-item"
								v-for="role in user.roles"
								:key="role"
							>
								{{ capitalizeText(role) }}
							</b-list-group-item>
						</b-list-group></b-card-text
					></b-tab
				>
			</b-tabs>
		</b-card>
		<b-modal
			id="manage-user-roles-modal"
			:title="selectedUserTag"
			@ok="editRoles"
		>
			<b-form-group label="Current Roles" v-slot="{ ariaDescribedby }">
				<b-form-checkbox-group
					v-model="selectedRoles"
					:options="allRoles"
					:aria-describedby="ariaDescribedby"
					name="flavour-2a"
					stacked
				></b-form-checkbox-group>
			</b-form-group>
		</b-modal>
		<b-modal id="add-users-modal" title="Add User" @ok="login">
			<b-form-group :class="{ 'form-group--error': $v.addedUserEmail.$error }">
				<label for="email">Email:</label>
				<b-form-input
					id="email"
					v-model="addedUserEmail"
					placeholder="user@email.com"
				></b-form-input>
			</b-form-group>
		</b-modal>
		<b-modal id="delete-user-modal" title="Delete User" @ok="deleteUser">
			<p>Are you sure you would like to delete user {{ selectedUserTag }}</p>
		</b-modal>
	</section>
</template>

<script lang="ts">
import {
	CreateUserRequest,
	UpdateRolesRequest,
	User,
} from "@/models/AdministratorModels";
import { Validate } from "vuelidate-property-decorators";
import { validationMixin } from "vuelidate";
import { required } from "vuelidate/lib/validators";
import { Component, Vue } from "vue-property-decorator";
import { capitalizeString } from "@/mixins/stringUtils";
import { Roles } from "@/models/Enums";

@Component({
	mixins: [validationMixin],
})
export default class ManageUsers extends Vue {
	@Validate({
		required,
	})
	addedUserEmail = "";

	capitalizeText = capitalizeString;

	get users(): Array<User> {
		return this.$store.state.AdminModule.users || [];
	}

	get allRoles(): Array<string> {
		const roles: string[] = Object.values(Roles);
		return roles.map(role => this.capitalizeText(role));
	}

	selectedEmail: string = this.firstSelectedUser.email;
	selectedUserTag: string =
		this.firstSelectedUser.username ?? this.firstSelectedUser.email;
	selectedUser: User = this.firstSelectedUser;

	get currentUsername(): string {
		return this.$store.state.username;
	}

	get firstSelectedUser(): User {
		const filteredUsers = this.users.filter(
			user => user.username !== this.currentUsername
		)[0];
		if (filteredUsers) {
			return filteredUsers;
		}
		return this.users[0];
	}

	selectedRoles: string[] = this.allRoles.filter(role =>
		(this.selectedUser.roles ?? [])
			.map(userRole => this.capitalizeText(userRole))
			.includes(role)
	);

	setSelectedUser(user: User) {
		this.selectedEmail = user.email;
		this.selectedUserTag = user.username ?? user.email;
		this.selectedUser = user;
		this.selectedRoles = user.roles;
	}

	async editRoles() {
		if (!this.selectedRoles) {
			this.$toast.error("Please select roles");
			return;
		}

		const request: UpdateRolesRequest = {
			email: this.selectedEmail,
			roles: this.selectedRoles,
		};

		await this.$store.dispatch("updateRoles", request);
	}

	async login() {
		this.$v.$touch();

		if (this.$v.$invalid) {
			Vue.$toast.error("Please fix invalid fields.");
			return;
		}

		const register: CreateUserRequest = {
			email: this.addedUserEmail,
		};

		await this.$store.dispatch("setIsLoading", true);
		await this.$store.dispatch("createUser", register);
		await this.$store.dispatch("getUsers");
		await this.$store.dispatch("setIsLoading", false);
	}

	async deleteUser() {
		await this.$store.dispatch("setIsLoading", true);
		await this.$store.dispatch("deleteUser", this.selectedEmail);
		await this.$store.dispatch("getUsers");
		await this.$store.dispatch("setIsLoading", false);
	}

	async beforeCreate() {
		await this.$store.dispatch("setIsLoading", true);
		await this.$store.dispatch("getUsers");
		await this.$store.dispatch("setIsLoading", false);
	}

	// TODO: Unsubscribe on beforeDestroy
}
</script>

<style scoped lang="scss">
.manage-users {
	margin-top: 2em;

	h1 {
		text-align: center;
		margin-bottom: 1em;
	}

	.role-list-item {
		border: 0;
	}
}
</style>
