import type {
  ForgotPasswordRequest,
  LoginRequest,
  LoginResponse,
  RefreshTokenRequest,
  ResetPasswordRequest,
} from "../../models/account";
import { apiClient } from "../clients/api-client";

const AccountService = {
  login: async (loginRequest: LoginRequest) => {
    const { data } = await apiClient.post<LoginResponse>(
      "/login",
      loginRequest
    );

    return data;
  },

  refreshToken: async (refreshTokenRequest: RefreshTokenRequest) => {
    const { data } = await apiClient.post<LoginResponse>(
      "/refresh-token",
      refreshTokenRequest
    );

    return data;
  },

  forgotPassword: async (forgotPasswordRequest: ForgotPasswordRequest) => {
    await apiClient.post("/forgot-password", forgotPasswordRequest);
  },

  resetPassword: async (resetPasswordRequest: ResetPasswordRequest) => {
    await apiClient.post("/forgot-password", resetPasswordRequest);
  },
};

export { AccountService };
