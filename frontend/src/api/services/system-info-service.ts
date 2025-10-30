import type { Statistics } from "../../models/system-info";
import { apiClient } from "../clients/api-client";

const SystemInfoService = {
  getStatistics: async () => {
    const { data } = await apiClient.get<Statistics>("/system-info/statistics");

    return data;
  },
};

export { SystemInfoService };
