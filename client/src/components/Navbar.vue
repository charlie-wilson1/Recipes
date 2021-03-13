<template>
  <section class="navbar-section">
    <b-navbar toggleable="lg" type="dark" variant="dark">
      <b-navbar-brand href="/">Recipes</b-navbar-brand>
      <b-navbar-toggle target="nav-collapse"></b-navbar-toggle>
      <b-collapse id="nav-collapse" is-nav>
        <b-navbar-nav v-if="isLoggedIn">
          <b-nav-item href="/create">Create</b-nav-item>
        </b-navbar-nav>
        <b-navbar-nav class="ml-auto" v-if="isLoggedIn">
          <b-nav-item-dropdown :text="username" right>
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
import axios from "axios";
import { Component, Vue } from "vue-property-decorator";

@Component({})
export default class Navbar extends Vue {
  get isLoggedIn(): boolean {
    const loggedIn = this.$store.getters.isLoggedIn;

    if (loggedIn) {
      const token = this.$store.getters.token;
      delete axios.defaults.headers.common["Authorization"];
      axios.defaults.headers.common["Authorization"] = `Bearer ${token}`;
    }

    return loggedIn;
  }

  get username(): string {
    return `Hello ${this.$store.getters.username}`;
  }
}
</script>
