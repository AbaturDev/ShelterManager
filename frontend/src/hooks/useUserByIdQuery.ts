import { useQuery, type UseQueryOptions } from "@tanstack/react-query";
import { UserService } from "../api/services/user-service";

const fiveMin = 5 * 60 * 1000;

export const useUserByIdQuery = (
  userId: string,
  options?: Partial<UseQueryOptions>
) => {
  return useQuery({
    queryFn: () => UserService.getUserById(userId),
    queryKey: ["users", userId],
    staleTime: fiveMin,
    enabled: !!userId,
    ...options,
  });
};
