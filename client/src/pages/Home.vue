<template>
  <section class="loader vh-100 text-center d-flex" v-if="isLoading">
    <b-spinner label="Loading..."></b-spinner>
  </section>
  <section v-else class="recipe-home container">
    <RecipeContainer />
  </section>
</template>

<script lang="ts">
import { GetAllRecipesQuery } from "@/models/RecipeModels";
import { Component, Vue } from "vue-property-decorator";
import RecipeContainer from "../components/home/RecipeContainer.vue";

@Component({
  components: {
    RecipeContainer
  }
})
export default class Home extends Vue {
  async beforeCreate() {
    const query: GetAllRecipesQuery = {
      resultsPerPage: 9,
      startNumber: 0,
      searchQuery: undefined
    }

    await this.$store.dispatch("setIsLoading", true);
    await this.$store.dispatch("loadRecipeList", query);
    await this.$store.dispatch("setIsLoading", false);
  }

  get isLoading() {
    return this.$store.getters.isLoading;
  }

  beforeDestroy() {
    this.$store.dispatch("destroyRecipeList");
  }
}
</script>
