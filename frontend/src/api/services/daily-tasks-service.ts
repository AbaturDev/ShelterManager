import type { CreateDailyTaskEntry, DailyTask } from "../../models/daily-task";
import { apiClient } from "../clients/api-client";

const DailyTasksService = {
  getDailyTask: async (animalId: string, date: string) => {
    const { data } = await apiClient.get<DailyTask>(
      `/animals/${animalId}/daily-tasks`,
      { params: { date: date } }
    );

    return data;
  },

  createDailyTaskEntry: async (
    animalId: string,
    data: CreateDailyTaskEntry
  ) => {
    await apiClient.post(`/animals/${animalId}/daily-tasks/entries`, data);
  },

  endDailyTaskEntry: async (animalId: string, id: string) => {
    await apiClient.post(`/animals/${animalId}/daily-tasks/entries/${id}/end`);
  },

  deleteDailyTaskEntry: async (animalId: string, id: string) => {
    await apiClient.delete(`/animals/${animalId}/daily-tasks/entries/${id}`);
  },
};

export { DailyTasksService };
