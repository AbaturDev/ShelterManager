import type { Breed, CreateBreed } from "../../models/breed";
import type { PaginatedResponse, PaginationQuery } from "../../models/common";
import { apiClient } from "../clients/api-client";

const BreedService = {
  getBreeds: async (speciesId: string, args: PaginationQuery) => {
    const { data } = await apiClient.get<PaginatedResponse<Breed>>(
      `/species/${speciesId}/breeds`,
      {
        params: { ...args },
      }
    );

    return data;
  },

  createBreed: async (speciesId: string, data: CreateBreed) => {
    await apiClient.post(`/species/${speciesId}/breeds`, data);
  },

  deleteBreed: async (id: string, speciesId: string) => {
    await apiClient.delete(`/species/${speciesId}/breeds/${id}`);
  },
};

export { BreedService };
