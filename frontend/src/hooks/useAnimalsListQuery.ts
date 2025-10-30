import { useQuery } from "@tanstack/react-query";
import type { AnimalQuery } from "../models/animal";
import { AnimalService } from "../api/services/animals-service";

const fiveMin = 60 * 1000 * 5;
export const useAnimalsListQuery = (query: AnimalQuery) => {
  return useQuery({
    queryKey: [
      "animals",
      query.page,
      query.pageSize,
      query.name,
      query.sex,
      query.status,
    ],
    queryFn: () => AnimalService.getAnimals(query),
    staleTime: fiveMin,
  });
};
