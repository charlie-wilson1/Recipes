import { ActionTree } from "vuex";
import { AdminState } from "./state";
import { RootState } from "@/store/state";
import {
	Recipe,
	Ingredient,
	Instruction,
	// RecipeImage,
} from "@/models/RecipeModels";
import { storage } from "@/firebase/firebase";
import Vue from "vue";
import axios from "axios";
import { User } from "@/models/AdministratorModels";

const adminUrl = process.env.VUE_APP_IDENTITY_URL + "admin";

export const actions: ActionTree<AdminState, RootState> = {
	async getUsers({ commit }) {
		console.log("getUsers");
		axios
			.get(`${adminUrl}/users`)
			.then(response => {
				const users: Array<User> = response.data;
				commit("setUsers", users);
			})
			.catch(err => {
				console.log(err);
				Vue.$toast.error(
					"Could not find any users or roles. Please try again."
				);
			});
	},
};
