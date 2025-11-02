import { useQuery } from "@tanstack/react-query";
import type { PaginationQuery } from "../models/common";
import { AnimalService } from "../api/services/animals-service";

const fiveMin = 5 * 60 * 1000;

export const useAnimalFilesQuery = (id: string, query: PaginationQuery) => {
  return useQuery({
    queryFn: () => AnimalService.getAnimalFiles(id, query),
    queryKey: ["animals", id, "files", query.page, query.pageSize],
    staleTime: fiveMin,
  });
};
