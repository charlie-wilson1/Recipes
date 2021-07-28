export interface RootState {
	version: string;
	isLoading: boolean;
	token?: string;
	username?: string;
	email?: string;
	roles?: string[];
	tokenExpiration?: number;
	didToken?: string;
	magicId?: string;
}
