import { useQuery } from "@tanstack/react-query";
import { BreedService } from "../api/services/breed-service";

const fiveMin = 60 * 1000 * 5;

export const useBreedsListQuery = (
  speciesId: string,
  page: number,
  pageSize: number
) => {
  return useQuery({
    queryKey: [`species/${speciesId}/breeds`, page, pageSize],
    queryFn: () =>
      BreedService.getBreeds(speciesId, { page: page, pageSize: pageSize }),
    staleTime: fiveMin,
    enabled: !!speciesId,
  });
};
