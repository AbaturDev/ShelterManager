import { VStack, Link, Flex, Text } from "@chakra-ui/react";
import { FaHome, FaHeart, FaPaw } from "react-icons/fa";
import { MdEvent } from "react-icons/md";
import { GiButterfly } from "react-icons/gi";
import { useTranslation } from "react-i18next";

export const SideBar = () => {
  const { t } = useTranslation();

  const links = [
    { name: t("navigation.home"), href: "/", icon: <FaHome /> },
    { name: t("navigation.animals"), href: "/animals", icon: <FaPaw /> },
    { name: t("navigation.species"), href: "/species", icon: <GiButterfly /> },
    { name: t("navigation.events"), href: "/events", icon: <MdEvent /> },
    { name: t("navigation.adoptions"), href: "/adoptions", icon: <FaHeart /> },
  ];

  return (
    <Flex
      direction="column"
      justify="space-between"
      h="100vh"
      w="200px"
      color="white"
      p={4}
    >
      <VStack align="stretch" spaceY={3}>
        {links.map((link) => (
          <Link
            key={link.name}
            href={link.href}
            _hover={{ bg: "green.600" }}
            px={3}
            py={2}
            borderRadius="md"
          >
            <Flex align="center" gapX={1}>
              {link.icon}
              <Text>{link.name}</Text>
            </Flex>
          </Link>
        ))}
      </VStack>
    </Flex>
  );
};
