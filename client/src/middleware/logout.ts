import store from "@/store/store";
import { NavigationGuardNext, Route } from "vue-router";


export const handleLogout = (next: NavigationGuardNext<Vue>) => {
  store.dispatch("logout");
  next("/Login");
}

export const matchesLogoutMeta = (to: Route): boolean => {
  return to.matched.some(record => record.meta.logout);
}
