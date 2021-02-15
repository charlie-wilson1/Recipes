<template lang="html">
  <section class="recipe-description">
    <b-img
      v-if="recipe.image"
      center
      thumbnail
      fluid
      v-bind:src="recipe.image.src"
      :alt="recipe.name"
    ></b-img>
    <h1 class="text-center">{{ recipe.name }}</h1>
    <b-row class="time-wrapper">
      <b-col class="text-center">
        <span class="mdi mdi-bowl-mix-outline mdi-48px"></span
        >{{ recipe.prepTime }}
      </b-col>
      <b-col class="text-center">
        <span class="mdi mdi-stove mdi-48px"></span>{{ recipe.cookTime }}
      </b-col>
      <b-col class="text-center">
        <span class="mdi mdi-timer-outline mdi-48px"></span
        >{{ recipe.totalTime }}
      </b-col>
    </b-row>
    <b-row>
      <b-col>
        <h3>Instructions</h3>
        <OrderedListGroup :values="instructions" isIndexed="true" />
      </b-col>
      <b-col>
        <h3>Ingredients</h3>
        <b-list-group class="ingredients">
          <b-list-group-item
            v-for="ingredient in recipe.ingredients"
            :key="ingredient.name"
          >
            <span>{{ ingredient.name }}</span>
            <span class="float-right"
              >{{ ingredient.quantity }}
              {{ getUnitName(ingredient.unit) }}</span
            >
          </b-list-group-item>
        </b-list-group>
      </b-col>
    </b-row>
  </section>
</template>

<script lang="ts">
import { Component, Prop, Vue } from "vue-property-decorator";
import { Recipe } from "@/models/RecipeModels";
import { Units } from "@/models/Enums";
import OrderedListGroup from "../shared/OrderedListGroup.vue";

@Component({
  components: {
    OrderedListGroup
  }
})
export default class RecipeDescription extends Vue {
  @Prop({ required: true })
  recipe!: Recipe;

  public getUnitName(unit: keyof typeof Units): string {
    return Units[unit];
  }

  get instructions() {
    return this.recipe.instructions.map(i => i.description);
  }
}
</script>

<style scoped lang="scss">
@import "public/css/values.scss";

.recipe-description {
  height: $recipe-container-text-height;
  width: 102%;
  overflow-y: scroll;
  overflow-x: hidden;
  padding-bottom: 100px;
  padding-left: 10px;
  padding-right: 10px;
  padding-top: 0;
  padding-bottom: 50px;

  &::-webkit-scrollbar {
    width: 5px;
  }

  &::-webkit-scrollbar-track {
    background: #f1f1f1;
  }

  &::-webkit-scrollbar-thumb {
    background: #888;
    outline: 1px solid slategrey;
  }

  &::-webkit-scrollbar-thumb:hover {
    background: #555;
  }

  h1 {
    margin-bottom: 10px;
    font-size: 3em;
    font-weight: 200;
  }

  h3 {
    margin-bottom: 20px;
    font-size: 2em;
    font-weight: 300;
  }

  .time-wrapper {
    font-size: 2em;
    margin-bottom: 20px;

    .mdi {
      margin-right: 10px;
    }
  }

  .label {
    font-weight: bold;
  }

  .img-thumbnail {
    height: 300px;
    width: 100%;
    overflow: hidden;
    object-fit: cover;
    margin-bottom: 20px;
  }

  .list-group-item {
    border: none;
    padding-left: 0px;
  }

  .list-group-index {
    font-weight: 600;
  }
}
</style>
