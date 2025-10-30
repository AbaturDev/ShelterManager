import { Box, Flex, Heading, Text, VStack } from "@chakra-ui/react";
import { useTranslation } from "react-i18next";
import { Dashboard } from "../components/homepage";

export const HomePage = () => {
  const { t } = useTranslation();

  return (
    <Flex w="100%" justifyContent={"center"}>
      <VStack gap={10}>
        <VStack gap={5}>
          <Heading size={"5xl"}>{t("homepage.title")}</Heading>
          <Heading size="3xl" color={"blackAlpha.600"}>
            {t("homepage.subtitle")}
          </Heading>
        </VStack>
        <Box padding={20}>
          <Dashboard />
        </Box>
        <Text>{t("homepage.thanks")}</Text>
      </VStack>
    </Flex>
  );
};
