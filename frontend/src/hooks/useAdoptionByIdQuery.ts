import { useQuery } from "@tanstack/react-query";
import { AdoptionService } from "../api/services/adoption-service";

const fiveMin = 60 * 1000 * 5;

export const useAdoptionByIdQuery = (id: string) => {
  return useQuery({
    queryKey: ["adoptions", id],
    queryFn: () => AdoptionService.getAdoptionById(id),
    staleTime: fiveMin,
  });
};
