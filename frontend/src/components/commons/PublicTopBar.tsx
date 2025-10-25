import { Flex, Image, Text, Box } from "@chakra-ui/react";
import { LanguageSelector } from "./LanguageSelector";

export const PublicTopBar = () => {
  return (
    <Flex
      as="header"
      bg="green.400"
      color="white"
      boxShadow="md"
      px={6}
      py={3}
      align="center"
      justify="space-between"
      position="sticky"
      top={0}
      zIndex={100}
    >
      <Flex align="center" gap={3}>
        <Image src="../../logo.png" alt="ShelterManager" boxSize="50px" />
        <Text fontSize="lg" fontWeight="bold" whiteSpace="nowrap">
          ShelterManager
        </Text>
      </Flex>
      <Box>
        <LanguageSelector />
      </Box>
    </Flex>
  );
};
