export interface CreateUserRequest {
	email: string;
}

export interface UpdateRolesRequest {
	email: string;
	roles: string[];
}

export interface User {
	id: string;
	username: string;
	email: string;
	roles: string[];
}
