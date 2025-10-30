import { useQuery } from "@tanstack/react-query";
import { SystemInfoService } from "../api/services/system-info-service";

const fiveMin = 5 * 60 * 1000;

export const useStatisticsQuery = () => {
  return useQuery({
    queryKey: ["events", "animals", "adoptions"],
    queryFn: () => SystemInfoService.getStatistics(),
    staleTime: fiveMin,
  });
};
