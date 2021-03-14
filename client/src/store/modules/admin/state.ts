import { User } from "@/models/AdministratorModels";

export interface AdminState {
	users: Array<User>;
	allRoles: Array<string>;
}

export const state: AdminState = {
	users: [],
	allRoles: [],
};
