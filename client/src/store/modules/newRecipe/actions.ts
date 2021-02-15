import { ActionTree } from "vuex";
import { NewRecipeState } from "./state";
import { RootState } from "@/store/state";
import {
  Recipe,
  Ingredient,
  Instruction,
  RecipeImage
} from "@/models/RecipeModels";
import { storage } from "@/database/firebase";
import Vue from "vue";
import axios from "axios";

const recipesUrl = process.env.VUE_APP_WEB_API_URL + "recipes";

export const actions: ActionTree<NewRecipeState, RootState> = {
  createNewRecipe({ commit }) {
    commit("createNewRecipe");
  },

  async setRecipeById({ commit }, recipeId: string) {
    axios
      .get(`${recipesUrl}/${recipeId}`)
      .then(response => {
        const recipe: Recipe = response.data;
        commit("setRecipeById", recipe);
      })
      .catch(err => {
        console.log(err);
        Vue.$toast.error("Could not find recipe. Please try again.");
      });
  },

  createDefaultIngredient({ commit }) {
    commit("createDefaultIngredient");
  },

  createDefaultInstruction({ commit }) {
    commit("createDefaultInstruction");
  },

  setSelectedIngredient({ commit }, index: number) {
    commit("setSelectedIngredient", index);
  },

  setSelectedInstruction({ commit }, index: number) {
    commit("setSelectedInstruction", index);
  },

  async insertRecipe(_, recipe: Recipe) {
    axios
      .post(recipesUrl, {
        data: recipe
      })
      .then(() => {
        Vue.$toast.success("Saved successfully!");
      })
      .catch(err => {
        console.log(err);
        Vue.$toast.error("Could not save recipe. Please try again.");
      });
  },

  async uploadRecipeImage({ commit, rootState }, image: File) {
    const unixTimestamp = Math.round(new Date().getTime() / 1000);
    const fileName = `${unixTimestamp}_${image.name}`;

    const storageRef = storage
      .ref("ajax-recipes")
      .child("images")
      .child(fileName);

    try {
      await storageRef.put(image);
    } catch {
      Vue.$toast.error("Could not save image. Please try again.");
    }

    const fileURL = await storageRef.getDownloadURL();

    const img: RecipeImage = {
      src: fileURL,
      fileName: image.name,
      fullName: fileName
    };

    axios
      .post(`${recipesUrl}/image`, {
        data: img
      })
      .then(() => {
        Vue.$toast.success("Saved successfully!");
      })
      .catch(err => {
        console.log(err);
        Vue.$toast.error("Could not save image. Please try again.");
      });

    commit("uploadRecipeImage", img);
  },

  insertIngredient({ commit }, ingredient: Ingredient) {
    commit("insertIngredient", ingredient);
  },

  insertIngredientNote({ commit }, note: string) {
    commit("insertIngredientNote", note);
  },

  insertInstruction({ commit }, instruction: Instruction) {
    commit("insertInstruction", instruction);
  },

  updateRecipe({ commit }, recipe: Recipe) {
    axios
      .put(recipesUrl, {
        data: recipe
      })
      .then(() => {
        Vue.$toast.success("Saved successfully!");
        commit("updateRecipe", recipe);
      })
      .catch(err => {
        console.log(err);
        Vue.$toast.error("Could not save recipe. Please try again.");
      });
  },

  updateInstruction(
    { commit },
    payload: { instruction: Instruction; index: number }
  ) {
    commit("updateInstruction", payload);
  },

  removeIngredient({ commit }, index: number) {
    commit("removeIngredient", index);
  },

  removeInstruction({ commit }, index: number) {
    commit("removeInstruction", index);
  },

  destroyNewRecipe({ commit }) {
    commit("destroyNewRecipe");
  },

  async deleteImage({ commit, rootState }, fileName: string) {
    try {
      await storage
        .ref("ajax-recipes")
        .child("images")
        .child(fileName)
        .delete();
    } catch (err) {
      console.log(err);
      Vue.$toast.error("Failed to delete image. Please try again.");
      return;
    }

    axios
      .delete(`${recipesUrl}/image`, {
        data: fileName
      })
      .then(() => {
        commit("deleteImage");
        Vue.$toast.success("Deleted successfully!");
      })
      .catch(err => {
        console.log(err);
        Vue.$toast.error("Failed to delete image. Please try again.");
      });
  }
};
