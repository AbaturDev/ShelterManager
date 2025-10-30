import { Flex, Text } from "@chakra-ui/react";
import { useTranslation } from "react-i18next";
import { RegisterUserDialog } from "./RegisterUserDialog";

export const UsersHeader = () => {
  const { t } = useTranslation();

  return (
    <Flex justifyContent={"space-between"} alignItems={"center"} mb={4}>
      <Text fontSize={"4xl"} fontWeight={"bold"}>
        {t("user.list.header")}
      </Text>
      <RegisterUserDialog />
    </Flex>
  );
};
