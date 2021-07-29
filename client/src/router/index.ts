import Vue from "vue";
import VueRouter, { RouteConfig } from "vue-router";
import Home from "@/pages/Home.vue";
import Authenticate from "@/pages/Auth/Authenticate.vue";
import NotFound from "@/pages/NotFound.vue";
import CreateRecipe from "@/pages/Recipe/CreateRecipe.vue";
import Make from "@/pages/Recipe/Make.vue";
import Login from "@/pages/Auth/Login.vue";
import Profile from "@/pages/Auth/Profile.vue";
import ManageUsers from "@/pages/Admin/ManageUsers.vue";
import { handleLogin, matchesLoginMeta } from "@/middleware/login";
import { handleLogout, matchesLogoutMeta } from "@/middleware/logout";
import {
	handleUnauthorized,
	matchesUnauthorizedMeta,
} from "@/middleware/unauthorized";

Vue.use(VueRouter);

const routes: Array<RouteConfig> = [
	{
		path: "/login",
		name: "login",
		component: Login,
	},
	{
		path: "/authenticate",
		name: "authenticate",
		component: Authenticate,
	},
	{
		path: "/make/:recipe_id",
		name: "make",
		component: Make,
		meta: {
			requiresLogin: true,
		},
	},
	{
		path: "/create",
		name: "create",
		component: CreateRecipe,
		meta: {
			requiresLogin: true,
		},
	},
	{
		path: "/edit/:recipe_id",
		name: "edit",
		component: CreateRecipe,
		meta: {
			requiresLogin: true,
		},
	},
	{
		path: "/admin/manage-users",
		name: "manageUsers",
		component: ManageUsers,
		meta: {
			requiresLogin: true,
			requiresAdmin: true,
		},
	},
	{
		path: "/logout",
		name: "logout",
		meta: {
			logout: true,
		},
	},
	{
		path: "/home",
		name: "home",
		component: Home,
		meta: {
			requiresLogin: true,
		},
	},
	{
		path: "/profile",
		name: "profile",
		component: Profile,
		meta: {
			requiresLogin: true,
		},
	},
	{
		path: "/not-found",
		name: "notFound",
		component: NotFound,
		meta: {
			requiresLogin: true,
		},
	},
	{
		path: "/",
		redirect: "home",
		meta: {
			requiresLogin: true,
		},
	},
	{
		path: "*",
		redirect: "/not-found",
	},
];

const router = new VueRouter({
	mode: "history",
	routes,
});

router.beforeEach(async (to, _, next) => {
	if (matchesLogoutMeta(to)) {
		handleLogout(next);
	}
	if (matchesLoginMeta(to)) {
		await handleLogin(next);
	}
	if (matchesUnauthorizedMeta(to)) {
		handleUnauthorized(next);
	}

	next();
});

export default router;
