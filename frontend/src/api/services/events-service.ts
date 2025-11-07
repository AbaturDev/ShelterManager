import type { PaginatedResponse } from "../../models/common";
import type {
  CreateEvent,
  Event,
  EventQuery,
  UpdateEvent,
} from "../../models/event";
import { apiClient } from "../clients/api-client";

const EventsService = {
  getEvents: async (args: EventQuery) => {
    const { data } = await apiClient.get<PaginatedResponse<Event>>(`/events`, {
      params: { ...args },
    });

    return data;
  },

  getEventById: async (id: string) => {
    const { data } = await apiClient.get<Event>(`/events/${id}`);

    return data;
  },

  endEvent: async (id: string) => {
    await apiClient.post(`/events/${id}/end`);
  },

  createEvent: async (data: CreateEvent) => {
    await apiClient.post(`/events`, data);
  },

  updateEvent: async (id: string, data: UpdateEvent) => {
    await apiClient.put(`/events/${id}`, data);
  },

  deleteEvent: async (id: string) => {
    await apiClient.delete(`/events/${id}`);
  },
};

export { EventsService };
