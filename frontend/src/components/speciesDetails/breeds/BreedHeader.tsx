import { Flex, Heading } from "@chakra-ui/react";
import { AddBreedDialog } from "./AddBreedDialog";
import { useTranslation } from "react-i18next";

interface BreedHeaderProps {
  speciesId: string;
}

export const BreedHeader = ({ speciesId }: BreedHeaderProps) => {
  const { t } = useTranslation();

  return (
    <Flex justifyContent={"space-between"} align={"center"} width={"100%"}>
      <Heading>{t("breeds.header")}</Heading>
      <AddBreedDialog speciesId={speciesId} />
    </Flex>
  );
};
