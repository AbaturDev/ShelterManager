import { useQuery } from "@tanstack/react-query";
import { AnimalService } from "../api/services/animals-service";
import { useEffect, useState } from "react";
import { AxiosError } from "axios";

const retryAttempts = 3;

export const useAnimalImage = (id: string) => {
  const [imageUrl, setImageUrl] = useState<string | null>(null);

  const { data, isLoading, error } = useQuery({
    queryKey: ["animal-image", id],
    queryFn: () => AnimalService.getAnimalProfileImage(id),
    staleTime: 10 * 60 * 1000,
    enabled: !!id,
    retry: (failCount, error) => {
      if (error instanceof AxiosError) {
        const status = error.response?.status;

        if (status === 500) {
          return false;
        }
      }

      return failCount < retryAttempts;
    },
  });

  useEffect(() => {
    if (data && data instanceof Blob) {
      const url = URL.createObjectURL(data);
      setImageUrl(url);

      return () => {
        URL.revokeObjectURL(url);
      };
    }
  }, [data]);

  return { imageUrl, isLoading, error };
};
