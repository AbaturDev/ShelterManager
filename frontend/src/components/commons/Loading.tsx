import { Spinner, Text, VStack } from "@chakra-ui/react";
import { useTranslation } from "react-i18next";

export const Loading = () => {
  const { t } = useTranslation();

  return (
    <VStack colorPalette="teal">
      <Spinner color="colorPalette.600" />
      <Text color="colorPalette.600">{t("loading")}</Text>
    </VStack>
  );
};
