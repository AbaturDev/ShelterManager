import { Flex, Text } from "@chakra-ui/react";
import { useTranslation } from "react-i18next";
import { AddSpeciesDialog } from ".";

export const SpeciesHeader = () => {
  const { t } = useTranslation();

  return (
    <Flex justifyContent={"space-between"} alignItems={"center"} mb={4}>
      <Text fontSize={"4xl"} fontWeight={"bold"}>
        {t("species.header")}
      </Text>
      <AddSpeciesDialog />
    </Flex>
  );
};
