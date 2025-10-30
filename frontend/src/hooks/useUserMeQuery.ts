import { useQuery } from "@tanstack/react-query";
import { UserService } from "../api/services/user-service";

const fiveMin = 5 * 60 * 1000;

export const useUserMeQuery = () => {
  return useQuery({
    queryFn: () => UserService.getMe(),
    queryKey: ["users", "me"],
    staleTime: fiveMin,
  });
};
