import { useQuery, type UseQueryOptions } from "@tanstack/react-query";
import { AnimalService } from "../api/services/animals-service";

const fiveMin = 5 * 60 * 1000;

export const useAnimalByIdQuery = (
  animalId: string,
  options?: Partial<UseQueryOptions>
) => {
  return useQuery({
    queryFn: () => AnimalService.getAnimalById(animalId),
    queryKey: ["animals", animalId],
    staleTime: fiveMin,
    enabled: !!animalId,
    ...options,
  });
};
