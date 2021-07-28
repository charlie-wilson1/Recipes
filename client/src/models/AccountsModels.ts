export interface RegisterUserCommand {
	email: string;
}

export interface LoginRequest {
	didToken: string | null;
	redirect?: string;
}

export interface MagicTokenRequest {
	email: string;
	redirect: string;
}

export interface UpdateCurrentUserCommand {
	username: string;
	email: string;
}

export interface TokenResponse {
	publicAddress: string;
	roles: string[];
	username: string;
	token: string;
	magicId: string;
	tokenExpiration: number;
}
