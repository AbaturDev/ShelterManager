import type {
  Animal,
  AnimalQuery,
  CreateAnimal,
  EditAnimal,
} from "../../models/animal";
import type { PaginatedResponse, PaginationQuery } from "../../models/common";
import type { FileResponse } from "../../models/file";
import { apiClient } from "../clients/api-client";

const AnimalService = {
  getAnimalById: async (id: string) => {
    const { data } = await apiClient.get<Animal>(`/animals/${id}`);

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

  editAnimal: async (id: string, data: EditAnimal) => {
    await apiClient.put(`/animals/${id}`, data);
  },

  uploadAnimalProfileImage: async (id: string, file: File) => {
    const formData = new FormData();
    formData.append("file", file);

    await apiClient.post(`/animals/${id}/profile-image`, formData, {
      headers: { "Content-Type": "multipart/form-data" },
    });
  },

  getAnimalFiles: async (id: string, args: PaginationQuery) => {
    const { data } = await apiClient.get<PaginatedResponse<FileResponse>>(
      `/animals/${id}/files`,
      {
        params: {
          ...args,
        },
      }
    );

    return data;
  },

  uploadAnimalFile: async (id: string, file: File) => {
    const formData = new FormData();
    formData.append("file", file);

    await apiClient.post(`/animals/${id}/files`, formData, {
      headers: { "Content-Type": "multipart/form-data" },
    });
  },

  getAnimalBlobFile: async (id: string, fileName: string): Promise<Blob> => {
    const response = await apiClient.get(`/animals/${id}/files/${fileName}`, {
      responseType: "blob",
    });
    return response.data;
  },

  deleteAnimalFile: async (id: string, fileName: string) => {
    await apiClient.delete(`/animals/${id}/files/${fileName}`);
  },
};

export { AnimalService };
