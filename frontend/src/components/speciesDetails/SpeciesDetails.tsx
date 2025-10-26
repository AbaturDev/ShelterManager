import {
  Box,
  Button,
  Card,
  Heading,
  HStack,
  Icon,
  Separator,
  Stack,
  Text,
} from "@chakra-ui/react";
import { useSpeciesDetailsQuery } from "../../hooks/useSpeciesDetailsQuery";
import { Loading } from "../commons";
import type { Species } from "../../models/species";
import { useTranslation } from "react-i18next";
import { MdDelete } from "react-icons/md";
import { GiButterfly } from "react-icons/gi";
import { useState } from "react";
import { DeleteSpeciesDialog } from "../species";
import { useNavigate } from "react-router-dom";
import { BreedHeader, BreedsList } from "./breeds";

interface SpeciesDetailsProps {
  id: string;
}

export const SpeciesDetails = ({ id }: SpeciesDetailsProps) => {
  const { t } = useTranslation();
  const navigate = useNavigate();
  const { data, isLoading, error } = useSpeciesDetailsQuery(id);
  const [isDeleteOpen, setIsDeleteOpen] = useState(false);

  if (isLoading) return <Loading />;
  if (error) return <Text>{t("species.error")}</Text>;

  const species = data as Species;

  return (
    <>
      <Stack p={6} gap={6} align="center">
        <Card.Root
          maxW="5xl"
          w="full"
          shadow="2xl"
          borderRadius="2xl"
          _hover={{ shadow: "3xl", transform: "scale(1.01)" }}
          transition="all 0.25s ease"
        >
          <Card.Header>
            <HStack justify="space-between" align="center">
              <HStack>
                <Icon as={GiButterfly} boxSize={8} color="green.400" />
                <Heading size="2xl" fontWeight="bold">
                  {species.name}
                </Heading>
              </HStack>
              <Button
                colorPalette="red"
                variant="solid"
                onClick={() => setIsDeleteOpen(true)}
              >
                <Icon as={MdDelete} />
                {t("species.delete")}
              </Button>
            </HStack>
          </Card.Header>

          <Card.Body>
            <Stack>
              <Text fontSize="lg">
                <strong>{t("species.createdAt")}:</strong>{" "}
                {new Date(species.createdAt).toLocaleString()}
              </Text>
              <Text fontSize="lg">
                <strong>{t("species.updatedAt")}:</strong>{" "}
                {new Date(species.updatedAt).toLocaleString()}
              </Text>
            </Stack>
          </Card.Body>
          <Separator padding={3} />
          <Card.Footer>
            <Box w="full" spaceY={5}>
              <BreedHeader speciesId={id} />
              <BreedsList speciesId={id} />
            </Box>
          </Card.Footer>
        </Card.Root>
      </Stack>

      <DeleteSpeciesDialog
        id={species.id}
        isOpen={isDeleteOpen}
        onClose={() => setIsDeleteOpen(false)}
        onSuccess={() => {
          setIsDeleteOpen(false);
          navigate("/species");
        }}
      />
    </>
  );
};
