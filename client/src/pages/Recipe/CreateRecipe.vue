<template>
  <div>
    <section class="loader vh-100 text-center d-flex" v-if="isLoading">
      <b-spinner label="Loading..."></b-spinner>
    </section>
    <section class="create-recipe container">
      <h1 class="text-center">{{ title }}</h1>
      <b-form @submit.prevent="handleSubmit(currentRecipe)">
        <b-form-row>
          <b-col md="6">
            <b-form-group
              label="Name"
              label-for="name"
              :class="{ 'form-group--error': $v.currentRecipe.name.$error }"
            >
              <b-form-input
                class="form__input"
                type="text"
                id="name"
                placeholder="Name"
                v-model.trim="$v.currentRecipe.name.$model"
              ></b-form-input>
              <div
                class="error"
                v-if="
                  !$v.currentRecipe.name.required &&
                    $v.currentRecipe.name.$error
                "
              >
                Field is required
              </div>
            </b-form-group>
          </b-col>
          <b-col md="6">
            <b-form-group label="Image" label-for="image">
              <b-form-file
                ref="recipeImage"
                placeholder="Choose a file or drop it here..."
                drop-placeholder="Drop file here..."
                @change="handleFileUpload"
              ></b-form-file>
              <span v-if="currentRecipe.image">
                <span class="mr-2">
                  <b-icon
                    icon="trash"
                    scale="1"
                    @click="handleDeleteImage(currentRecipe.image.fullName)"
                  ></b-icon>
                </span>
                {{ currentRecipe.image.fileName }}
              </span>
            </b-form-group>
          </b-col>
        </b-form-row>

        <b-form-row>
          <b-col md="4">
            <b-form-group
              label="Prep Time"
              label-for="prepTime"
              :class="{ 'form-group--error': $v.currentRecipe.prepTime.$error }"
            >
              <b-form-input
                class="form__input"
                v-model.number="$v.currentRecipe.prepTime.$model"
                type="number"
                id="prepTime"
                placeholder="Prep"
                min="1"
                required
              ></b-form-input>
              <div
                class="error"
                v-if="
                  !$v.currentRecipe.name.required &&
                    $v.currentRecipe.name.$error
                "
              >
                Field is required
              </div>
            </b-form-group>
          </b-col>
          <b-col md="4">
            <b-form-group
              label="Cook Time"
              label-for="cookTime"
              :class="{ 'form-group--error': $v.currentRecipe.cookTime.$error }"
            >
              <b-form-input
                class="form__input"
                v-model.number="$v.currentRecipe.cookTime.$model"
                type="number"
                id="cookTime"
                placeholder="Cook"
                min="1"
                required
              ></b-form-input>
              <div
                class="error"
                v-if="
                  !$v.currentRecipe.name.required &&
                    $v.currentRecipe.name.$error
                "
              >
                Field is required
              </div>
            </b-form-group>
          </b-col>
          <b-col md="4">
            <b-form-group label="Total Time" label-for="totalTime">
              <b-form-input
                :value="totalTime"
                type="text"
                id="totalTime"
                readonly
                placeholder="Total"
                min="0"
                required
              ></b-form-input>
            </b-form-group>
          </b-col>
        </b-form-row>
        <hr />
        <IngredientsForm :_ingredients="currentRecipe.ingredients" />
        <hr />
        <InstructionsForm :instructions="currentRecipe.instructions" />
        <hr />
        <b-form-row>
          <b-col md="12">
            <b-form-group label="Notes" label-for="notes">
              <b-form-textarea
                class="form__input"
                id="notes"
                v-model.trim="currentRecipe.notes"
              >
              </b-form-textarea>
            </b-form-group>
          </b-col>
        </b-form-row>
        <b-form-group class="text-center button-wrapper">
          <b-button type="submit" variant="success mr-2">Submit </b-button>
          <b-button variant="primary" v-b-modal.clear-modal>Clear </b-button>
        </b-form-group>
      </b-form>
    </section>
    <section class="clear-modal">
      <b-modal centered id="clear-modal" title="Clear Recipe" @ok="handleReset">
        <div class="d-block text-center">
          <p>
            Are you sure you would like to clear the recipe? Changes will not be
            saved.
          </p>
        </div>
      </b-modal>
    </section>
  </div>
</template>

<script lang="ts">
import { Component, Vue } from "vue-property-decorator";
import { Validate } from "vuelidate-property-decorators";
import { validationMixin } from "vuelidate";
import { required, minValue } from "vuelidate/lib/validators";
import { Ingredient, Recipe } from "@/models/RecipeModels";
import { defaultRecipe } from "@/models/DefaultModels";
import CreateList from "@/components/create/CreateList.vue";
import IngredientsForm from "@/components/create/IngredientsForm.vue";
import InstructionsForm from "@/components/create/InstructionsForm.vue";

@Component({
  components: {
    CreateList,
    IngredientsForm,
    InstructionsForm
  },
  mixins: [validationMixin]
})
export default class CreateRecipe extends Vue {
  get isEditMode(): boolean {
    return this.$route.path.toLowerCase().includes("edit");
  }

  get recipeId(): number | undefined {
    if (this.isEditMode) {
      return parseInt(this.$route.params.recipe_id);
    }
    return undefined;
  }

  get title(): string {
    return this.isEditMode ? "Edit Recipe" : "Create Recipe";
  }

  get isLoading(): boolean {
    return this.$store.getters.isLoading;
  }

  @Validate({
    name: { required },
    ingredients: { required },
    instructions: { required },
    prepTime: { required, minValue: minValue(1) },
    cookTime: { required, minValue: minValue(1) }
  })
  get currentRecipe(): Recipe {
    let recipe: Recipe = this.$store.getters.recipe;

    if (!recipe) {
      recipe = defaultRecipe;
    }

    return recipe;
  }

  get totalTime(): number {
    return +this.currentRecipe.prepTime + +this.currentRecipe.cookTime;
  }

  getIngredientString(ingredients: Ingredient[]) {
    return ingredients.map(ingredient => {
      return {
        defaultValue: ingredient.name,
        additionalValue: `${ingredient.quantity} ${ingredient.unitId}`,
        notes: ingredient.notes
      };
    });
  }

  getSimpleStringForCreateList(defaultValues: string[]) {
    return defaultValues.map(val => {
      return {
        defaultValue: val
      };
    });
  }

  // eslint-disable-next-line
  handleFileUpload(event: any) {
    const files: FileList = event.target.files;
    const image: File = files[0];
    this.$store.dispatch("uploadRecipeImage", image);
  }

  handleSubmit(recipe: Recipe) {
    this.$v.$touch();

    if (this.$v.$invalid) {
      Vue.$toast.error("Please fix invalid fields.");
      return;
    }

    if (this.isEditMode) {
      // eslint-disable-next-line
      recipe.id = this.recipeId!;
      this.$store.dispatch("updateRecipe", recipe);
    } else {
      this.$store.dispatch("insertRecipe", recipe);
    }
    this.$v.$reset();
  }

  handleReset() {
    if (!this.isEditMode) {
      this.$store.dispatch("setRecipeById", this.recipeId);
    } else {
      this.$store.dispatch("createNewRecipe");
    }
    this.$v.$reset();
  }

  handleDeleteImage(fullName: string) {
    this.$store.dispatch("deleteImage", fullName);
  }

  beforeCreate() {
    this.$store.dispatch("setIsLoading", true);
  }

  created() {
    if (this.isEditMode) {
      this.$store.dispatch("setRecipeById", this.recipeId);
    } else {
      this.$store.dispatch("createNewRecipe");
    }
    this.$store.dispatch("setIsLoading", false);
  }

  beforeDestroy() {
    this.$store.dispatch("destroyNewRecipe");
  }
}
</script>

<style scoped lang="scss">
.create-recipe {
  margin-top: 30px;
  background-color: #e9ecef;
  border-radius: 20px;
  padding: 30px 5%;

  .button-wrapper {
    .btn {
      width: 90px;
      height: 50px;
      font-size: 1.2em;
    }
  }

  .error {
    color: red;
  }

  .form-group--error {
    input {
      border-color: red;
    }
  }
}
</style>
