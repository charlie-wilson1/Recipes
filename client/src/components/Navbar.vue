<template>
  <section class="navbar-section">
    <b-navbar toggleable="lg" type="dark" variant="dark">
      <b-navbar-brand href="/">Recipes</b-navbar-brand>
      <div v-if="isLoggedIn">
        <b-navbar-toggle target="nav-collapse"></b-navbar-toggle>
        <b-collapse id="nav-collapse" is-nav>
          <b-navbar-nav>
            <b-nav-item href="/create" class="mr-4">Create</b-nav-item>
            <b-nav-item href="/logout" class="mr-4">Logout</b-nav-item>
          </b-navbar-nav>
        </b-collapse>
      </div>
    </b-navbar>
  </section>
</template>

<script lang="ts">
import { Component, Vue } from "vue-property-decorator";

@Component({})
export default class Navbar extends Vue {
  get isLoggedIn(): boolean {
    return this.$store.getters.isLoggedIn;
  }
}
</script>
