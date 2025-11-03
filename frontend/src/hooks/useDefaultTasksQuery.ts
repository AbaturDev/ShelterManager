import { useQuery } from "@tanstack/react-query";
import { DailyTasksService } from "../api/services/daily-tasks-service";

const fiveMin = 5 * 60 * 1000;

export const useDefaultTasksQuery = (animalId: string) => {
  return useQuery({
    queryKey: ["default-tasks", animalId],
    queryFn: () => DailyTasksService.getDefaultDailyTaskEntries(animalId),
    staleTime: fiveMin,
  });
};
