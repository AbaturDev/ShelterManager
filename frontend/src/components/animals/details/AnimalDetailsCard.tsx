import {
  Badge,
  Box,
  Card,
  FileUpload,
  Flex,
  Heading,
  HStack,
  Icon,
  Separator,
  Stack,
  Tabs,
  Text,
  VStack,
} from "@chakra-ui/react";
import { useAnimalByIdQuery } from "../../../hooks/useAnimalByIdQuery";
import { Loading } from "../../commons";
import { useTranslation } from "react-i18next";
import { FaPaw } from "react-icons/fa";
import type { Animal } from "../../../models/animal";
import { AnimalBadge } from "../AnimalBadge";
import { AnimalImage } from "../AnimalImage";
import { FilesTab } from "./tabs";
import { MdEdit } from "react-icons/md";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { AnimalService } from "../../../api/services/animals-service";
import { toaster } from "../../ui/toaster";
import { DailyTaskTab } from "../../daily-tasks";

interface AnimalsDetailsCardProps {
  id: string;
}

export const AnimalDetailsCard = ({ id }: AnimalsDetailsCardProps) => {
  const { t } = useTranslation();
  const queryClient = useQueryClient();

  const uploadMutation = useMutation({
    mutationFn: (file: File) =>
      AnimalService.uploadAnimalProfileImage(id, file),
    onSuccess: () => {
      queryClient.invalidateQueries({
        queryKey: ["animal-image", id],
      });
      toaster.create({
        type: "success",
        title: t("success"),
        description: t("animals.details.profileImage.toast.success"),
        closable: true,
      });
    },
    onError: () => {
      toaster.create({
        type: "error",
        title: t("error"),
        description: t("animals.details.profileImage.toast.error"),
        closable: true,
      });
    },
  });

  const { data, isLoading: isAnimalLoading, error } = useAnimalByIdQuery(id);

  if (isAnimalLoading) return <Loading />;
  if (error || data === undefined) return <Text color={"red"}>{t("???")}</Text>;

  const animal = data as Animal;

  return (
    <Stack p={6} gap={6} align="center">
      <Card.Root w="full" maxW="4xl">
        <Card.Header>
          <Flex w="100%" justifyContent={"space-between"} align={"center"}>
            <HStack>
              <Icon as={FaPaw} boxSize={8} color="green.400" />
              <Heading size="2xl" fontWeight="bold">
                {animal.name}
              </Heading>
            </HStack>
            <AnimalBadge status={animal.status} />
          </Flex>
        </Card.Header>
        <Card.Body>
          <Flex justifyContent={"space-between"}>
            <Flex maxH="150px" maxW="150px" gap={1}>
              <AnimalImage animal={animal} />
              <FileUpload.Root
                accept="image/*"
                onFileChange={(uploadFiles) => {
                  uploadMutation.mutate(uploadFiles.acceptedFiles[0]);
                }}
              >
                <FileUpload.HiddenInput />
                <FileUpload.Trigger asChild>
                  <Icon
                    as={MdEdit}
                    boxSize={6}
                    color="white"
                    bg="blue.500"
                    borderRadius="full"
                    p={1}
                    bottom={2}
                    right={2}
                    cursor="pointer"
                    _hover={{ bg: "blue.600" }}
                  />
                </FileUpload.Trigger>
              </FileUpload.Root>
            </Flex>

            <VStack align="start">
              <Text>
                <strong>{t("animals.details.age")}:</strong>{" "}
                {animal.age ? (
                  <>
                    {animal.age} {t("animals.details.years")}
                  </>
                ) : (
                  <>{t("animals.unknown")}</>
                )}
              </Text>
              <Text>
                <strong>{t("animals.sex.title")}:</strong>{" "}
                {t(`animals.sex.${animal.sex}`)}
              </Text>
              <Text>
                <strong>{t("animals.details.admissionDate")}:</strong>{" "}
                <Text as="span">
                  {new Date(animal.admissionDate).toLocaleDateString()}
                </Text>
              </Text>
              <Text>
                <strong>{t("animals.details.species")}:</strong>{" "}
                {animal.species.name}
              </Text>
              <Text>
                <strong>{t("animals.details.breed")}:</strong>{" "}
                {animal.species.breed.name}
              </Text>
            </VStack>
            <Flex />
          </Flex>

          <Separator my={6} />

          <Box>
            <Heading size="md" mb={2}>
              {t("animals.details.description")}
            </Heading>
            <Text fontStyle="italic" color="gray.700">
              {animal.description || t("animals.details.noDescription")}
            </Text>
          </Box>
        </Card.Body>

        <Card.Footer>
          <Tabs.Root defaultValue="details" w="full">
            <Tabs.List justifyContent={"space-between"}>
              <Tabs.Trigger value="details">
                {t("animals.details.tabs.details")}
              </Tabs.Trigger>
              <Tabs.Trigger value="files">
                {t("animals.details.tabs.files")}
              </Tabs.Trigger>
              <Tabs.Trigger value="dailyTasks">
                {t("animals.details.tabs.dailyTasks")}
              </Tabs.Trigger>
            </Tabs.List>
            <Tabs.Content value="details">
              <Flex justifyContent={"start"} direction={"column"} gap={5}>
                <Text>
                  <strong>Id:</strong> <Text as="span">{animal.id}</Text>
                </Text>
                <Text>
                  <strong>{t("createdAt")}:</strong>{" "}
                  <Text as="span">
                    {new Date(animal.createdAt).toLocaleString()}
                  </Text>
                </Text>
                <Text>
                  <strong>{t("updatedAt")}:</strong>{" "}
                  <Text as="span">
                    {new Date(animal.updatedAt).toLocaleString()}
                  </Text>
                </Text>
                <Text>
                  <strong>{t("animals.details.tabs.hasImage")}:</strong>{" "}
                  <Text as="span">
                    {animal.imagePath ? (
                      <Badge colorPalette={"green"}>{t("yes")}</Badge>
                    ) : (
                      <Badge colorPalette={"grey"}>{t("no")}</Badge>
                    )}
                  </Text>
                </Text>
              </Flex>
            </Tabs.Content>
            <Tabs.Content value="files">
              <FilesTab animalId={animal.id} />
            </Tabs.Content>
            <Tabs.Content value="dailyTasks">
              <DailyTaskTab animalId={animal.id} />
            </Tabs.Content>
          </Tabs.Root>
        </Card.Footer>
      </Card.Root>
    </Stack>
  );
};
