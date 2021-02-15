<template>
  <div id="app">
    <Navbar />
    <router-view />
  </div>
</template>

<script lang="ts">
import { Component, Vue } from "vue-property-decorator";
import Navbar from "./components/Navbar.vue";

@Component({
  components: {
    Navbar
  }
})
export default class App extends Vue {}
</script>

<style lang="scss">
@import "node_modules/bootstrap/scss/bootstrap.scss";

#nav {
  padding: 30px;

  a {
    font-weight: bold;
    color: #2c3e50;

    &.router-link-exact-active {
      color: #42b983;
    }
  }
}

.list-group-flush > .list-group-item {
  border-width: 1px;
}

.list-group-item + .list-group-item {
  border-top-width: 1px;
}

.spinner-border {
  margin: auto;
  width: 4rem;
  height: 4rem;
}
</style>
