import { useQuery } from "@tanstack/react-query";
import { DailyTasksService } from "../api/services/daily-tasks-service";

const fiveMin = 60 * 1000 * 5;

export const useDailyTaskQuery = (animalId: string, date: string) => {
  return useQuery({
    queryKey: [animalId, `daily-task`, date],
    queryFn: () => DailyTasksService.getDailyTask(animalId, date),
    staleTime: fiveMin,
    retry: () => {
      return false;
    },
  });
};
