import type { User } from "../../models/user";
import { apiClient } from "../clients/api-client";

const UserService = {
  getMe: async () => {
    const { data } = await apiClient.get<User>("/users/me");

    return data;
  },
};

export { UserService };
