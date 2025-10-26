import type {
  ChangePasswordRequest,
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

  forgotPassword: async (
    forgotPasswordRequest: ForgotPasswordRequest,
    lang: string
  ) => {
    await apiClient.post("/forgot-password", forgotPasswordRequest, {
      headers: {
        "Accept-Language": lang,
      },
    });
  },

  resetPassword: async (resetPasswordRequest: ResetPasswordRequest) => {
    await apiClient.post("/reset-password", resetPasswordRequest);
  },

  changePassword: async (changePasswordRequest: ChangePasswordRequest) => {
    await apiClient.post("/change-password", changePasswordRequest);
  },
};

export { AccountService };
