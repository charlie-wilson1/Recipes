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
