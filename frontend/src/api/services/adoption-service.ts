import type {
  Adoption,
  AdoptionDetails,
  AdoptionQuery,
  CreateAdoption,
  UpdateAdoption,
} from "../../models/adoptions";
import type { PaginatedResponse } from "../../models/common";
import { apiClient } from "../clients/api-client";

const AdoptionService = {
  getAdoptions: async (args: AdoptionQuery) => {
    const { data } = await apiClient.get<PaginatedResponse<Adoption>>(
      `/adoptions`,
      {
        params: { ...args },
      }
    );

    return data;
  },

  getAdoptionById: async (id: string) => {
    const { data } = await apiClient.get<AdoptionDetails>(`/adoptions/${id}`);

    return data;
  },

  getAdoptionAgreement: async (id: string): Promise<Blob> => {
    const response = await apiClient.get(
      `/adoptions/${id}/adoption-agreement​`,
      {
        responseType: "blob",
      }
    );
    return response.data;
  },

  createAdoption: async (data: CreateAdoption) => {
    await apiClient.post(`/adoptions`, data);
  },

  updateAdoption: async (id: string, data: UpdateAdoption) => {
    await apiClient.put(`/adoptions/${id}`, data);
  },

  deleteAdoption: async (id: string) => {
    await apiClient.delete(`/adoptions/${id}`);
  },
};

export { AdoptionService };
