import { useQuery } from "@tanstack/react-query";
import { SpeciesService } from "../api/services/species-service";

const fiveMin = 60 * 1000 * 5;

export const useSpeciesDetailsQuery = (id: string) => {
  return useQuery({
    queryKey: ["species", id],
    queryFn: () => SpeciesService.getSpeciesById(id),
    staleTime: fiveMin,
  });
};
