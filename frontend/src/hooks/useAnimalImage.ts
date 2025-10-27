import { useQuery } from "@tanstack/react-query";
import { AnimalService } from "../api/services/animals-service";
import { useEffect, useState } from "react";

export const useAnimalImage = (id: string) => {
  const [imageUrl, setImageUrl] = useState<string | null>(null);

  const { data, isLoading, error } = useQuery({
    queryKey: ["animal-image", id],
    queryFn: () => AnimalService.getAnimalProfileImage(id),
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
