import Vue from "vue";
import store from "@/store/store";
import { NavigationGuardNext, Route } from "vue-router";

export const handleUnauthorized = (next: NavigationGuardNext<Vue>) => {
	Vue.$toast.warning("Unauthorized");
	next("/home");
};

export const matchesUnauthorizedMeta = (to: Route): boolean => {
	return (
		to.matched.some(record => record.meta.requiresAdmin) &&
		!store.getters.isAdmin
	);
};
