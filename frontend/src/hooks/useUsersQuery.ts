import { useQuery } from "@tanstack/react-query";
import { UserService } from "../api/services/user-service";
import type { PaginationQuery } from "../models/common";

const fiveMin = 5 * 60 * 1000;

export const useUsersQuery = (args: PaginationQuery) => {
  return useQuery({
    queryFn: () => UserService.getUsers(args),
    queryKey: ["users", args.page, args.pageSize],
    staleTime: fiveMin,
  });
};
