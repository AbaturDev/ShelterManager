import { useQuery } from "@tanstack/react-query";
import { UserService } from "../api/services/user-service";

const fiveMin = 5 * 60 * 1000;

export const useUserByIdQuery = (userId: string) => {
  return useQuery({
    queryFn: () => UserService.getUserById(userId),
    queryKey: ["users", userId],
    staleTime: fiveMin,
  });
};
