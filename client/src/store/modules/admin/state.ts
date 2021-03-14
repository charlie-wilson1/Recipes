import { User } from "@/models/AdministratorModels";

export interface AdminState {
	users: Array<User>;
}

export const state: AdminState = {
	users: [],
};
