import {
  type ManageDailyTaskEntry,
  type DailyTask,
  type DefaultDailyTaskEntry,
} from "../../models/daily-task";
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
    data: ManageDailyTaskEntry
  ) => {
    await apiClient.post(`/animals/${animalId}/daily-tasks/entries`, data);
  },

  endDailyTaskEntry: async (animalId: string, id: string) => {
    await apiClient.post(`/animals/${animalId}/daily-tasks/entries/${id}/end`);
  },

  deleteDailyTaskEntry: async (animalId: string, id: string) => {
    await apiClient.delete(`/animals/${animalId}/daily-tasks/entries/${id}`);
  },

  getDefaultDailyTaskEntries: async (animalId: string) => {
    const { data } = await apiClient.get<DefaultDailyTaskEntry[]>(
      `/animals/${animalId}/daily-tasks/default-entries`
    );

    return data;
  },

  createDefaultDailyTaskEntry: async (
    animalId: string,
    data: ManageDailyTaskEntry
  ) => {
    await apiClient.post(
      `/animals/${animalId}/daily-tasks/default-entries`,
      data
    );
  },

  updateDefaultDailyTaskEntry: async (
    animalId: string,
    id: string,
    data: ManageDailyTaskEntry
  ) => {
    await apiClient.put(
      `/animals/${animalId}/daily-tasks/default-entries/${id}`,
      data
    );
  },

  deleteDefaultDailyTaskEntry: async (animalId: string, id: string) => {
    await apiClient.delete(
      `/animals/${animalId}/daily-tasks/default-entries/${id}`
    );
  },
};

export { DailyTasksService };
