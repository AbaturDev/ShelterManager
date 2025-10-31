import {
  Card,
  Image,
  Text,
  Badge,
  Flex,
  Heading,
  Icon,
  Center,
  Spinner,
  HStack,
} from "@chakra-ui/react";
import type { AnimalStatusType } from "../../../models/animal";
import { PossibleAnimalStatus } from "../../../models/animal";

import animalPlaceholder from "../../../assets/animal-placeholder.png";

import { useTranslation } from "react-i18next";
import type { Animal } from "../../../models/animal";
import { MdDelete, MdEdit } from "react-icons/md";
import { useNavigate } from "react-router-dom";
import { useState } from "react";
import { DeleteAnimalDialog } from "../DeleteAnimalDialog";
import { useAnimalImage } from "../../../hooks/useAnimalImage";
import { EditAnimalDialog } from "../EditAnimalDialog";

interface AnimalCardProps {
  animal: Animal;
}

export const AnimalCard = ({ animal }: AnimalCardProps) => {
  const { t } = useTranslation();
  const navigate = useNavigate();

  const { imageUrl, isLoading } = animal.imagePath
    ? useAnimalImage(animal.id)
    : { imageUrl: null, isLoading: false };
  const displayImage =
    animal.imagePath && imageUrl ? imageUrl : animalPlaceholder;
  const showSpinner = animal.imagePath && isLoading;

  const [isDeleteOpen, setIsDeleteOpen] = useState(false);
  const [isEditOpen, setIsEditOpen] = useState(false);

  const BadgeMapper = (status: AnimalStatusType) => {
    switch (status) {
      case PossibleAnimalStatus.InShelter:
        return (
          <Badge variant={"solid"} colorPalette={"blue"}>
            {t("animals.status.inShelter")}
          </Badge>
        );
      case PossibleAnimalStatus.Adopted:
        return (
          <Badge variant={"solid"} colorPalette={"green"}>
            {t("animals.status.adopted")}
          </Badge>
        );
      case PossibleAnimalStatus.Died:
        return (
          <Badge variant={"solid"} colorScheme={"red"}>
            {t("animals.status.died")}
          </Badge>
        );
    }
  };

  return (
    <>
      <Card.Root
        height="100%"
        bg="white"
        shadow="sm"
        borderRadius="2xl"
        overflow="hidden"
        transition="transform 0.2s"
        _hover={{ transform: "scale(1.02)", shadow: "lg" }}
        onDoubleClick={() => navigate(`/animals/${animal.id}`)}
      >
        <Card.Header p={0} height="200px" overflow="hidden">
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
        </Card.Header>

        <Card.Body p={4}>
          <Flex justify="space-between" align="center" mb={2}>
            <Heading size="md">{animal.name}</Heading>
            {BadgeMapper(animal.status)}
          </Flex>

          <Text fontSize="sm" color="gray.600">
            {animal.species.name} • {animal.species.breed.name}
          </Text>

          <Text fontSize="sm" mt={2}>
            {t("animals.sex.title")}: {t(`animals.sex.${animal.sex}`)}
          </Text>

          <Text fontSize="sm">
            {t("animals.list.age")}:{" "}
            {animal.age ? (
              <>
                {animal.age} {t("animals.list.years")}
              </>
            ) : (
              <>{t("animals.unknown")}</>
            )}
          </Text>

          <Flex
            mt={2}
            w="100%"
            justifyContent={"space-between"}
            align={"center"}
          >
            <Text fontSize="xs" color="gray.500">
              {t("animals.list.admissionDate")}:{" "}
              {new Date(animal.admissionDate).toLocaleDateString()}
            </Text>
            <HStack>
              <Icon
                as={MdEdit}
                boxSize={5}
                onClick={() => setIsEditOpen(true)}
                _hover={{ cursor: "pointer" }}
              />
              <Icon
                as={MdDelete}
                boxSize={5}
                onClick={() => setIsDeleteOpen(true)}
                _hover={{ cursor: "pointer" }}
              />
            </HStack>
          </Flex>
        </Card.Body>
      </Card.Root>
      <DeleteAnimalDialog
        isOpen={isDeleteOpen}
        id={animal.id}
        onClose={() => setIsDeleteOpen(false)}
        onSuccess={() => setIsDeleteOpen(false)}
      />
      <EditAnimalDialog
        isOpen={isEditOpen}
        id={animal.id}
        onClose={() => setIsEditOpen(false)}
        onSuccess={() => setIsEditOpen(false)}
      />
    </>
  );
};
