export interface AdminRegisterUserCommand {
	email: string;
}

export interface UpdateRolesCommand {
	username: string;
	roles: Array<string>;
}

export interface AdminResetUserPasswordCommand {
	email: string;
}

export interface User {
	id: string;
	username: string;
	roles: Array<string>;
}
