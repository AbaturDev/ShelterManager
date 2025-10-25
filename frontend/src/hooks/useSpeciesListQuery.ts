import { useQuery } from "@tanstack/react-query";
import { SpeciesService } from "../api/services/species-service";

const fiveMin = 60 * 1000 * 5;

export const useSpeciesListQuery = (page: number, pageSize: number) => {
  return useQuery({
    queryKey: ["species", page, pageSize],
    queryFn: () =>
      SpeciesService.getSpecies({ page: page, pageSize: pageSize }),
    staleTime: fiveMin,
  });
};
