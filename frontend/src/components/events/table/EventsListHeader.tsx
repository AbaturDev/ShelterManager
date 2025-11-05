import { Button, Flex, Heading, HStack } from "@chakra-ui/react";
import { useTranslation } from "react-i18next";
import { AddEventDialog } from "./AddEventDialog";

export const EventsListHeader = () => {
  const { t } = useTranslation();

  return (
    <Flex direction={"column"} gap={7}>
      <Heading size="3xl">{t("events.list.header")}</Heading>
      <Flex w="100%" justifyContent={"center"}>
        <HStack w="80%" justifyContent={"space-between"}>
          <AddEventDialog />
          <HStack>
            <Button>Zwierzęta</Button>
            <Button>Zakończone</Button>
            <Button>Filtruj</Button>
          </HStack>
        </HStack>
      </Flex>
    </Flex>
  );
};
