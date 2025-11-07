import { useQuery } from "@tanstack/react-query";
import { EventsService } from "../api/services/events-service";

const fiveMin = 60 * 1000 * 5;
export const useEventDetailsQuery = (id: string) => {
  return useQuery({
    queryKey: ["events", id],
    queryFn: () => EventsService.getEventById(id),
    staleTime: fiveMin,
    enabled: !!id,
  });
};
