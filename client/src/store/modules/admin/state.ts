import { User } from "@/models/AdministratorModels";

export interface AdminState {
	users: User[];
}

export const state: AdminState = {
	users: [],
};
