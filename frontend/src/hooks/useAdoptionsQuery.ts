import { useQuery } from "@tanstack/react-query";
import type { AdoptionQuery } from "../models/adoptions";
import { AdoptionService } from "../api/services/adoption-service";

const fiveMin = 60 * 1000 * 5;

export const useAdoptionsQuery = (query: AdoptionQuery) => {
  return useQuery({
    queryKey: [
      "adoptions",
      query.page,
      query.pageSize,
      query.animalName,
      query.status,
      query.animalIds,
    ],
    queryFn: () => AdoptionService.getAdoptions(query),
    staleTime: fiveMin,
  });
};
