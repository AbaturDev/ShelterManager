import type { Animal, AnimalQuery, CreateAnimal } from "../../models/animal";
import type { PaginatedResponse } from "../../models/common";
import { apiClient } from "../clients/api-client";

const AnimalService = {
  getAnimalById: async (id: string) => {
    const { data } = await apiClient.get<Animal>(`/aniamls/${id}`);

    return data;
  },

  getAnimals: async (args: AnimalQuery) => {
    const { data } = await apiClient.get<PaginatedResponse<Animal>>(
      "/animals",
      {
        params: {
          ...args,
        },
      }
    );

    return data;
  },

  deleteAnimal: async (id: string) => {
    await apiClient.delete(`/animals/${id}`);
  },

  getAnimalProfileImage: async (id: string): Promise<Blob> => {
    const response = await apiClient.get(`/animals/${id}/profile-image`, {
      responseType: "blob",
    });
    return response.data;
  },

  createAnimal: async (data: CreateAnimal) => {
    await apiClient.post("/animals", data);
  },
};

export { AnimalService };
