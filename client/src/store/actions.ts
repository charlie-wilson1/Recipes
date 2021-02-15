import Vue from "vue";
import { ActionTree } from "vuex";
import { RootState } from "@/store/state";
import jwt from "jsonwebtoken";
import router from "@/router/index";
import {
  UpdateCurrentUserCommand,
  UpdatePasswordCommand,
  ResetPasswordCommand,
  ConfirmResetPasswordCommand,
  TokenResponse,
  LoginRequest
} from "@/models/AccountsModels";
import {
  AdminRegisterUserCommand,
  UpdateRolesCommand,
  AdminResetUserPasswordCommand
} from "@/models/AdministratorModels";
import axios from "axios";

const accountsUrl = process.env.VUE_APP_IDENTITY_URL + "accounts/";
const adminUrl = process.env.VUE_APP_IDENTITY_URL + "admin/";

export const actions: ActionTree<RootState, RootState> = {
  setIsLoading: ({ commit }, isLoading: boolean) => {
    commit("setIsLoading", isLoading);
  },

  async register({ commit }, payload: LoginRequest) {
    axios
      .post(accountsUrl + "register", payload.command)
      .then(response => {
        const token = response.data.accessToken as string;
        console.log(token);
        // eslint-disable-next-line
        const decodedToken: any = jwt.decode(token);

        const data: TokenResponse = {
          token: token,
          username: decodedToken.unique_name,
          roles: decodedToken.role,
          tokenExpiration: decodedToken.exp,
          refreshToken: response.data.refreshToken
        };

        commit("setUserVariables", data);
        
        delete axios.defaults.headers.common["Authorization"];
        axios.defaults.headers.common["Authorization"] = `Bearer ${token}`;

        if (payload.redirect) {
          router.push(payload.redirect);
        } else {
          router.push("home");
        }
      })
      .catch(err => {
        Vue.$toast.error(`Error registering: ${err}`);
      });
  },

  async login({ commit }, payload: LoginRequest) {
    axios
      .post(accountsUrl + "login", payload.command)
      .then(response => {
        const token = response.data.accessToken as string;
        console.log(token);
        // eslint-disable-next-line
        const decodedToken: any = jwt.decode(token);

        const data: TokenResponse = {
          token: token,
          username: decodedToken.unique_name,
          roles: decodedToken.role,
          tokenExpiration: decodedToken.exp,
          refreshToken: response.data.refreshToken
        };

        commit("setUserVariables", data);

        delete axios.defaults.headers.common["Authorization"];
        axios.defaults.headers.common["Authorization"] = `Bearer ${token}`;

        if (payload.redirect) {
          router.push(payload.redirect);
        } else {
          router.push("home");
        }
      })
      .catch(err => {
        Vue.$toast.error(`Error logging in: ${err}`);
      });
  },

  async getRoles({ commit }) {
    axios
      .get(accountsUrl + "roles")
      .then(response => {
        commit("updateRoles", response);
      })
      .catch(err => {
        Vue.$toast.error(`Error refreshing roles: ${err}`);
      });
  },

  async refreshJwtToken({ commit, rootState }) {
    axios
      .post(accountsUrl + "refresh", {
        data: {
          refreshToken: rootState.refreshToken,
          username: rootState.username
        }
      })
      .then(response => {
        const token = response.data.accessToken as string;
        console.log(token);
        // eslint-disable-next-line
        const decodedToken: any = jwt.decode(token);

        const data: TokenResponse = {
          token: token,
          username: decodedToken.unique_name,
          roles: decodedToken.role,
          tokenExpiration: decodedToken.exp,
          refreshToken: response.data.refreshToken
        };

        commit("setUserVariables", data);
        delete axios.defaults.headers.common["Authorization"];
        axios.defaults.headers.common["Authorization"] = `Bearer ${token}`;
      })
      .catch(err => {
        Vue.$toast.error(`Error logging in: ${err}`);
      });
  },

  async updateUser(_, command: UpdateCurrentUserCommand) {
    axios.post(accountsUrl + "UpdateCurrentUser", command).catch(err => {
      Vue.$toast.error(`Error updating user: ${err}`);
    });
  },

  async updatePassword(_, command: UpdatePasswordCommand) {
    axios.post(accountsUrl + "UpdatePassword", command).catch(err => {
      Vue.$toast.error(`Error updating password: ${err}`);
    });
  },

  async resetPassword(_, command: ResetPasswordCommand) {
    axios.post(accountsUrl + "ResetPassword", command).catch(err => {
      Vue.$toast.error(`Error resetting password: ${err}`);
    });
  },

  async confirmResetPassword(_, command: ConfirmResetPasswordCommand) {
    axios.post(accountsUrl + "ConfirmResetPassword", command).catch(err => {
      Vue.$toast.error(`Error resetting password: ${err}`);
    });
  },

  async adminRegister(_, command: AdminRegisterUserCommand) {
    axios.post(adminUrl + "Register", command).catch(err => {
      Vue.$toast.error(`Error registering new user: ${err}`);
    });
  },

  async adminUpdateRoles(_, command: UpdateRolesCommand) {
    axios.post(adminUrl + "login", command).catch(err => {
      Vue.$toast.error(`Error updating user roles: ${err}`);
    });
  },

  async adminResetUserPassword(_, command: AdminResetUserPasswordCommand) {
    axios.post(adminUrl + "Register", command).catch(err => {
      Vue.$toast.error(`Error registering new user: ${err}`);
    });
  },

  logout({ commit }) {
    delete axios.defaults.headers.common["Authorization"];
    commit("logout");
  }
};
