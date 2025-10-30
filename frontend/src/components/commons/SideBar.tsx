import { VStack, Link, Flex, Text } from "@chakra-ui/react";
import { FaHome, FaHeart, FaPaw, FaUsers } from "react-icons/fa";
import { MdEvent } from "react-icons/md";
import { GiButterfly } from "react-icons/gi";
import { useTranslation } from "react-i18next";
import { useAuth } from "../../utils";

export const SideBar = () => {
  const { t } = useTranslation();
  const auth = useAuth();

  const links = [
    { name: t("navigation.home"), href: "/", icon: <FaHome /> },
    { name: t("navigation.animals"), href: "/animals", icon: <FaPaw /> },
    { name: t("navigation.species"), href: "/species", icon: <GiButterfly /> },
    { name: t("navigation.events"), href: "/events", icon: <MdEvent /> },
    { name: t("navigation.adoptions"), href: "/adoptions", icon: <FaHeart /> },
  ];

  const adminLinks = [
    { name: t("navigation.users"), href: "/users", icon: <FaUsers /> },
  ];

  return (
    <Flex direction="column" h="100vh" w="200px" color="white" p={4} gapY={10}>
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
        {auth?.role === "Admin" &&
          adminLinks.map((link) => (
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
