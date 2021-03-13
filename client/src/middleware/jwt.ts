import store from "@/store/store";
import { NavigationGuardNext } from "vue-router";

export const handleTokenRefresh = (next: NavigationGuardNext<Vue>) => {
  store.dispatch("refreshJwtToken");
  next();
};

export const tokenExpired =
  store.getters.tokenExpiration <= Math.floor(Date.now() / 1000);
