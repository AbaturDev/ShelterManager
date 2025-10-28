import { Heading, Icon, VStack, Text } from "@chakra-ui/react";
import type { ReactNode } from "react";
import type { IconType } from "react-icons/lib";

interface StatCardProps {
  icon: IconType;
  label: string;
  value: number | string | ReactNode;
  color: string;
}

export const StatCard = ({ icon, label, value, color }: StatCardProps) => {
  return (
    <VStack
      boxShadow="md"
      borderRadius="2xl"
      p={6}
      minW={{ base: "full", md: "250px" }}
      align="center"
      transition="all 0.2s ease"
      _hover={{ transform: "translateY(-5px)", boxShadow: "xl" }}
    >
      <Icon as={icon} boxSize={10} color={color} />
      <Heading size="md">{label}</Heading>
      <Text fontSize="4xl" fontWeight="bold" color={color}>
        {value}
      </Text>
    </VStack>
  );
};
