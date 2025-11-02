import type { Animal } from "../../models/animal";
import animalPlaceholder from "../../assets/animal-placeholder.png";
import { useAnimalImage } from "../../hooks/useAnimalImage";
import { Center, Spinner, Image } from "@chakra-ui/react";

interface AnimalImageProps {
  animal: Animal;
}

export const AnimalImage = ({ animal }: AnimalImageProps) => {
  const { imageUrl, isLoading } = animal.imagePath
    ? useAnimalImage(animal.id)
    : { imageUrl: null, isLoading: false };
  const displayImage =
    animal.imagePath && imageUrl ? imageUrl : animalPlaceholder;
  const showSpinner = animal.imagePath && isLoading;

  return (
    <>
      {showSpinner ? (
        <Center height="100%" bg="gray.100">
          <Spinner size="xl" color="blue.500" />
        </Center>
      ) : (
        <Image
          src={displayImage}
          alt={animal.name}
          width="100%"
          height="100%"
          objectFit="cover"
        />
      )}
    </>
  );
};
