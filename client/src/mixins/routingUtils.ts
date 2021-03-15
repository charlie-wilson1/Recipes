import Vue from "vue";
import store from "@/store/store";
import router from "@/router/index";

export function redirect(message: string) {
	if (store.getters.isLoggedIn) {
		router.push("/home");
	} else {
		router.push("/login");
	}
	Vue.$toast.error(message);
}
