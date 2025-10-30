import { Button, Flex, Heading, Box } from "@chakra-ui/react";
import { useTranslation } from "react-i18next";
import { SearchInput } from "../../commons";
import { AddAnimalDialog } from "../create";

interface HeaderProps {
  onSearch: (query: string) => void;
}

export const Header = ({ onSearch }: HeaderProps) => {
  const { t } = useTranslation();

  return (
    <Flex
      justifyContent="space-between"
      alignItems="center"
      flexWrap="wrap"
      gap={4}
      mb={6}
      bg="white"
    >
      <Box flexShrink={0}>
        <Heading
          fontSize={{ base: "2xl", md: "4xl", lg: "5xl" }}
          fontWeight="bold"
        >
          {t("animals.list.title")}
        </Heading>
      </Box>

      <Box flex="1" minW="600px" maxW="800px">
        <SearchInput onSearch={onSearch} />
      </Box>

      <Box>
        <AddAnimalDialog />
      </Box>
    </Flex>
  );
};
