import type { PaginatedResponse, PaginationQuery } from "../../models/common";
import type { CreateSpecies, Species } from "../../models/species";
import { apiClient } from "../clients/api-client";

const SpeciesService = {
  getSpeciesById: async (id: string) => {
    const { data } = await apiClient.get<Species>(`/species/${id}`);

    return data;
  },

  getSpecies: async (args: PaginationQuery) => {
    const { data } = await apiClient.get<PaginatedResponse<Species>>(
      "/species",
      {
        params: {
          ...args,
        },
      }
    );

    return data;
  },

  createSpecies: async (data: CreateSpecies) => {
    await apiClient.post("/species", data);
  },

  deleteSpecies: async (id: string) => {
    await apiClient.delete(`/species/${id}`);
  },
};

export { SpeciesService };
