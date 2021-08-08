export interface RegisterUserRequest {
	email: string;
}

export interface LoginRequest {
	didToken?: string | null;
	redirect?: string;
	triedOnce?: boolean;
}

export interface MagicTokenRequest {
	email: string;
	redirect: string;
}

export interface UpdateUsernameRequest {
	username: string;
	email: string;
}

export interface TokenResponse {
	roles: string[];
	username: string;
	email: string;
	token: string;
	tokenExpiration: number;
}
