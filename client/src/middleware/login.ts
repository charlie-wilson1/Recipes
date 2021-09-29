import store from "@/store/store";
import { NavigationGuardNext, Route } from "vue-router";

export const handleLogin = async (next: NavigationGuardNext<Vue>) => {
	await store.dispatch("setDidToken");
	await store.dispatch("getJwtToken", { handleRedirect: false });
	await store.dispatch("setIsLoggedIn");
	await store.dispatch("createShoppingCart");
	next();
};

export const matchesLoginMeta = (to: Route): boolean => {
	return (
		to.matched.some(record => record.meta.requiresLogin) &&
		!store.getters.isLoggedIn
	);
};
