export interface RegisterUserCommand {
	email: string;
	username: string;
	password: string;
	confirmPassword: string;
}

export interface LoginCommand {
	username: string;
	password: string;
}

export interface LoginRequest {
	command: LoginCommand;
	redirect: string | undefined;
}

export interface UpdateCurrentUserCommand {
	username: string;
	email: string;
}

export interface UpdatePasswordCommand {
	currentPassword: string;
	newPassword: string;
	newPasswordConfirmation: string;
}

export interface ResetPasswordCommand {
	email: string;
}

export interface ConfirmResetPasswordCommand {
	email: string;
	resetToken: string;
	newPassword: string;
	newPasswordConfirmation: string;
}

export interface TokenResponse {
	token: string;
	username: string;
	roles: Array<string>;
	tokenExpiration: number;
	refreshToken: string;
}
