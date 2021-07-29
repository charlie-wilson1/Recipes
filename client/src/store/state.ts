export interface RootState {
	version: string;
	isLoading: boolean;
	isLoggedIn: boolean;
	token?: string;
	username?: string;
	email?: string;
	roles?: string[];
	tokenExpiration?: number;
	didToken?: string;
}
