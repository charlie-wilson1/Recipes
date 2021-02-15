export interface RootState {
  version: string;
  isLoading: boolean;
  token?: string;
  refreshToken?: string;
  username?: string;
  roles?: Array<string>;
  tokenExpiration?: number;
}
