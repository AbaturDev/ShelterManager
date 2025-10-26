import { useTranslation } from "react-i18next";
import { useUserMeQuery } from "../../hooks/useUserMeQuery";
import {
  CloseButton,
  Dialog,
  Portal,
  Text,
  Box,
  Spinner,
  VStack,
  Flex,
  Icon,
  HStack,
} from "@chakra-ui/react";
import { MdCheckCircle, MdWarning } from "react-icons/md";
import { FaUserCircle } from "react-icons/fa";

interface UserProfileDialogProps {
  isOpen: boolean;
  onClose: () => void;
}

export const UserProfile = ({ isOpen, onClose }: UserProfileDialogProps) => {
  const { t } = useTranslation();
  const { data, error, isLoading } = useUserMeQuery();

  return (
    <Portal>
      <Dialog.Root open={isOpen} onOpenChange={onClose} size={"lg"}>
        <Dialog.Backdrop />
        <Dialog.Positioner>
          <Dialog.Content maxW="md" rounded="xl" p={0}>
            <Dialog.Header
              fontWeight="semibold"
              fontSize="md"
              borderBottomWidth="1px"
              py={3}
              px={4}
            >
              {t("user.profile.name")}
            </Dialog.Header>

            <Dialog.CloseTrigger asChild top={1} right={1}>
              <CloseButton size="sm" />
            </Dialog.CloseTrigger>

            <Dialog.Body p={6}>
              {isLoading && (
                <Box textAlign="center" py={8}>
                  <Spinner size="lg" color="green.600" />
                </Box>
              )}

              {error && (
                <Text color="red.500" textAlign="center" py={4}>
                  {t("user.profile.error")}
                </Text>
              )}

              {data && (
                <VStack gap={4} align="stretch">
                  <HStack gap={4} pb={4} gapX={5} borderBottomWidth="1px">
                    <Icon as={FaUserCircle} boxSize={20} color="gray.400" />
                    <VStack align="start" gap={1}>
                      <Text fontSize="xl" fontWeight="bold">
                        {data.name} {data.surname}
                      </Text>
                      <Text fontSize="sm" color="gray.600">
                        {data.role}
                      </Text>
                    </VStack>
                  </HStack>

                  <HStack>
                    <Text fontSize={"sm"} fontWeight={"bold"}>
                      @:
                    </Text>
                    <Text
                      color="blue.600"
                      fontSize="sm"
                      _hover={{ textDecoration: "underline" }}
                      cursor="pointer"
                    >
                      {data.email}
                    </Text>
                  </HStack>

                  <Flex
                    mt={2}
                    p={3}
                    bg={data.mustChangePassword ? "orange.50" : "green.50"}
                    borderRadius="md"
                    align="center"
                    gap={2}
                  >
                    <Icon
                      as={data.mustChangePassword ? MdWarning : MdCheckCircle}
                      color={
                        data.mustChangePassword ? "orange.500" : "green.500"
                      }
                      boxSize={5}
                    />
                    <Text
                      fontSize="sm"
                      color={
                        data.mustChangePassword ? "orange.700" : "green.700"
                      }
                      fontWeight="medium"
                    >
                      {data.mustChangePassword
                        ? t("user.profile.password.notVerified")
                        : t("user.profile.password.verified")}
                    </Text>
                  </Flex>
                </VStack>
              )}
            </Dialog.Body>
          </Dialog.Content>{" "}
        </Dialog.Positioner>
      </Dialog.Root>
    </Portal>
  );
};
