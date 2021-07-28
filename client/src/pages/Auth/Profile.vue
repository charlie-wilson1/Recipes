<template lang="html">
	<section class="profile container">
		<h1>Profile</h1>
		<ProfileUpdateUser />
	</section>
</template>

<script lang="ts">
import { Component, Vue } from "vue-property-decorator";
import ProfileUpdateUser from "@/components/Auth/ProfileUpdateUser.vue";

@Component({
	components: {
		ProfileUpdateUser,
	},
})
export default class Profile extends Vue {}
</script>

<style scoped lang="scss">
.profile {
	&.container {
		margin-top: 2em;
	}

	h1 {
		text-align: center;
	}
}
</style>
