import { Card, Text, Flex, Heading, Icon, HStack } from "@chakra-ui/react";
import { useTranslation } from "react-i18next";
import type { Animal } from "../../../models/animal";
import { MdDelete, MdEdit } from "react-icons/md";
import { useNavigate } from "react-router-dom";
import { useState } from "react";
import { DeleteAnimalDialog } from "../DeleteAnimalDialog";
import { EditAnimalDialog } from "../EditAnimalDialog";
import { AnimalBadge } from "../AnimalBadge";
import { AnimalImage } from "../AnimalImage";

interface AnimalCardProps {
  animal: Animal;
}

export const AnimalCard = ({ animal }: AnimalCardProps) => {
  const { t } = useTranslation();
  const navigate = useNavigate();

  const [isDeleteOpen, setIsDeleteOpen] = useState(false);
  const [isEditOpen, setIsEditOpen] = useState(false);

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
          <AnimalImage animal={animal} />
        </Card.Header>

        <Card.Body p={4}>
          <Flex justify="space-between" align="center" mb={2}>
            <Heading size="md">{animal.name}</Heading>
            <AnimalBadge status={animal.status} />
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
