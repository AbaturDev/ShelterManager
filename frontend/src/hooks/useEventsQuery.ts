import { useQuery } from "@tanstack/react-query";
import type { EventQuery } from "../models/event";
import { EventsService } from "../api/services/events-service";

const fiveMin = 60 * 1000 * 5;
export const useEventsQuery = (query: EventQuery) => {
  return useQuery({
    queryKey: [
      "events",
      query.page,
      query.pageSize,
      query.isDone,
      query.animalIds,
      query.title,
    ],
    queryFn: () => EventsService.getEvents(query),
    staleTime: fiveMin,
  });
};
