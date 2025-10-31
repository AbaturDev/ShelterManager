import {
  Badge,
  Card,
  Flex,
  Heading,
  HStack,
  Icon,
  Stack,
  Text,
} from "@chakra-ui/react";
import { useAnimalByIdQuery } from "../../../hooks/useAnimalByIdQuery";
import { Loading } from "../../commons";
import { useTranslation } from "react-i18next";
import { FaPaw } from "react-icons/fa";

interface AnimalsDetailsCardProps {
  id: string;
}

export const AnimalsDetailsCard = ({ id }: AnimalsDetailsCardProps) => {
  const { t } = useTranslation();
  const { data, isLoading, error } = useAnimalByIdQuery(id);

  if (isLoading) return <Loading />;
  if (error || data === undefined) return <Text color={"red"}>{t("???")}</Text>;

  return (
    <Stack p={6} gap={6} align="center">
      <Card.Root w="full" maxW="5xl">
        <Card.Header>
          <Flex w="100%" justifyContent={"space-between"} align={"center"}>
            <HStack>
              <Icon as={FaPaw} boxSize={8} color="green.400" />
              <Heading size="2xl" fontWeight="bold">
                {data.name}
              </Heading>
              <Badge />
            </HStack>
          </Flex>
        </Card.Header>
      </Card.Root>
    </Stack>
  );
};
