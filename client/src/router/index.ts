import Vue from "vue";
import VueRouter, { RouteConfig } from "vue-router";
import Home from "@/pages/Home.vue";
import CreateRecipe from "@/pages/Recipe/CreateRecipe.vue";
import Make from "@/pages/Recipe/Make.vue";
import Login from "@/pages/Auth/Login.vue";
import Register from "@/pages/Auth/Register.vue";
import AdminRegister from "@/pages/Admin/AdminRegister.vue";
import { handleLogin, matchesLoginMeta } from "@/middleware/login";
import { handleLogout, matchesLogoutMeta } from "@/middleware/logout";
import {
  handleUnauthorized,
  matchesUnauthorizedMeta
} from "@/middleware/unauthorized";
import { handleTokenRefresh, tokenExpired } from "@/middleware/jwt";

Vue.use(VueRouter);

const routes: Array<RouteConfig> = [
  {
    path: "/login",
    name: "login",
    component: Login
  },
  {
    path: "/register",
    name: "register",
    component: Register
  },
  {
    path: "/make/:recipe_id",
    name: "make",
    component: Make,
    meta: {
      requiresLogin: true
    }
  },
  {
    path: "/create",
    name: "create",
    component: CreateRecipe,
    meta: {
      requiresLogin: true
    }
  },
  {
    path: "/edit/:recipe_id",
    name: "edit",
    component: CreateRecipe,
    meta: {
      requiresLogin: true
    }
  },
  {
    path: "/admin/register",
    name: "adminRegister",
    component: AdminRegister,
    meta: {
      requiresLogin: true,
      requiresAdmin: true
    }
  },
  {
    path: "/logout",
    name: "logout",
    meta: {
      logout: true
    }
  },
  {
    path: "/home",
    name: "home",
    component: Home,
    meta: {
      requiresLogin: true
    }
  },
  {
    path: "/",
    redirect: "home",
    meta: {
      requiresLogin: true
    }
  }
];

const router = new VueRouter({
  mode: "history",
  routes
});

router.beforeEach((to, _, next) => {
  if (matchesLogoutMeta(to)) {
    handleLogout(next);
  }
  if (matchesLoginMeta(to)) {
    handleLogin(next);
  }
  if (matchesUnauthorizedMeta(to)) {
    handleUnauthorized(next);
  }
  if (tokenExpired) {
    handleTokenRefresh(next);
  }

  next();
});

export default router;
