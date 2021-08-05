import store from "@/store/store";
import { NavigationGuardNext, Route } from "vue-router";

export const handleLogout = async (next: NavigationGuardNext<Vue>) => {
	await store.dispatch("logout");
	await store.dispatch("setIsLoggedIn");
	next("/Login");
};

export const matchesLogoutMeta = (to: Route): boolean => {
	return to.matched.some(record => record.meta.logout);
};
