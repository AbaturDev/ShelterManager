import { Flex, Image, Text, HStack } from "@chakra-ui/react";
import { LanguageSelector } from "./LanguageSelector";
import { NavLink } from "react-router-dom";
import { ProfileMenu } from "./ProfileMenu";

export const TopBar = () => {
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
      <NavLink to={"/"}>
        <Flex align="center" gap={3} cursor="pointer">
          <Image
            src="../../logo.png"
            alt="ShelterManager"
            boxSize="50px"
            mr={2}
          />
          <Text fontSize="lg" fontWeight="bold">
            ShelterManager
          </Text>
        </Flex>
      </NavLink>
      <HStack gap={3}>
        <LanguageSelector />
        <ProfileMenu />
      </HStack>
    </Flex>
  );
};
