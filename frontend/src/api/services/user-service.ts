import type { PaginatedResponse, PaginationQuery } from "../../models/common";
import type { User, UserDetails } from "../../models/user";
import { apiClient } from "../clients/api-client";

const UserService = {
  getMe: async () => {
    const { data } = await apiClient.get<UserDetails>("/users/me");

    return data;
  },

  getUserById: async (userId: string) => {
    const { data } = await apiClient.get<UserDetails>(`/users/${userId}`);

    return data;
  },

  getUsers: async (args: PaginationQuery) => {
    const { data } = await apiClient.get<PaginatedResponse<User>>("/users", {
      params: {
        ...args,
      },
    });

    return data;
  },

  deleteUser: async (userId: string) => {
    await apiClient.delete(`/users/${userId}`);
  },
};

export { UserService };
