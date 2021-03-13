import store from "@/store/store";
import { NavigationGuardNext, Route } from "vue-router";

export const handleLogin = (next: NavigationGuardNext<Vue>) => {
  next("/Login");
};

export const matchesLoginMeta = (to: Route): boolean => {
  return (
    to.matched.some(record => record.meta.requiresLogin) &&
    !store.getters.isLoggedIn
  );
};
