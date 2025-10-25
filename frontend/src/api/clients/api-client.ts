import axios from "axios";
import { AccountService } from "../services/account-service";

export const apiClient = axios.create({
  baseURL: import.meta.env.VITE_BACKEND_URL,
});

apiClient.interceptors.request.use(
  (request) => {
    const accessToken = localStorage.getItem("jwtToken");
    if (accessToken) {
      request.headers["Authorization"] = `Bearer ${accessToken}`;
    }
    return request;
  },
  (error) => {
    return Promise.reject(error);
  }
);

apiClient.interceptors.response.use(
  (response) => response,
  async (error) => {
    const originalRequest = error.config;
    if (error.response.status === 401 && !originalRequest._retry) {
      originalRequest._retry = true;
      try {
        const refreshToken = localStorage.getItem("refreshToken") as string;

        const response = await AccountService.refreshToken({
          refreshToken: refreshToken,
        });

        localStorage.setItem("jwtToken", response.jwtToken);
        localStorage.setItem("refreshToken", response.refreshToken);

        apiClient.defaults.headers.common[
          "Authorization"
        ] = `Bearer ${response.jwtToken}`;

        return apiClient(originalRequest);
      } catch (refreshError) {
        localStorage.removeItem("jwtToken");
        localStorage.removeItem("refreshToken");

        window.location.href = "/login";

        return Promise.reject(refreshError);
      }
    }
    return Promise.reject(error);
  }
);
