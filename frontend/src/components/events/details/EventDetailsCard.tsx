import {
  Badge,
  Box,
  Flex,
  Heading,
  HStack,
  Icon,
  Separator,
  Stack,
  Text,
  VStack,
} from "@chakra-ui/react";
import type { Event } from "../../../models/event";
import { MdLocationOn, MdPets } from "react-icons/md";
import { Loading } from "../../commons";
import { useTranslation } from "react-i18next";
import { FaUser, FaUserCheck } from "react-icons/fa";
import { FaSackDollar } from "react-icons/fa6";
import { GrCompliance } from "react-icons/gr";
import { CiCalendarDate } from "react-icons/ci";
import { useUserByIdQuery } from "../../../hooks/useUserByIdQuery";
import type { UserDetails } from "../../../models/user";

interface EventDetailsCardProps {
  event: Event | undefined;
  isLoading: boolean;
}

export const EventDetailsCard = ({
  event,
  isLoading,
}: EventDetailsCardProps) => {
  const { t } = useTranslation();

  const { data: employeeUserData } = useUserByIdQuery(event?.userId ?? "", {
    enabled: !!event?.userId,
  });
  const { data: endUserData } = useUserByIdQuery(
    event?.completedByUserId ?? "",
    {
      enabled: !!event?.completedByUserId,
    }
  );

  if (isLoading) return <Loading />;
  if (event === undefined)
    return <Text color="red">{t("events.details.error")}</Text>;

  const employeeUser = employeeUserData as UserDetails;
  const endUser = endUserData as UserDetails;

  const statusColor = event.isDone ? "green" : "orange";
  const statusLabel = event.isDone
    ? t("events.details.completed")
    : t("events.details.active");

  return (
    <Box maxW="700px" mx="auto" p={6} bg="white" rounded="2xl" shadow="md">
      <Stack>
        <Heading textAlign={"center"} size="3xl" marginBottom={1}>
          {event.title}
        </Heading>

        <HStack justify="space-between" align="center">
          <HStack>
            <Icon boxSize={6} as={CiCalendarDate} />
            <Text fontSize={"lg"} fontWeight={"semibold"}>
              {t("events.details.date")} {new Date(event.date).toLocaleString()}
            </Text>
          </HStack>
          <Badge colorPalette={statusColor}>{statusLabel}</Badge>
        </HStack>

        <Separator />

        <VStack align="start" gap={4}>
          <HStack>
            <Icon as={MdLocationOn} color="blue.500" />
            <Text fontWeight="semibold">{t("events.details.location")}</Text>
            <Text>{event.location}</Text>
          </HStack>

          <HStack>
            <Icon as={MdPets} color="purple.500" />
            <Text fontWeight="semibold">{t("events.details.animal")}</Text>
            <Text>{event.animalName}</Text>
          </HStack>

          {event.cost && (
            <HStack>
              <Icon as={FaSackDollar} />
              <Text fontWeight="semibold">{t("events.details.cost")}</Text>
              <Text>
                {event.cost.amount} {event.cost.currencyCode}
              </Text>
            </HStack>
          )}

          {event.userId && (
            <HStack>
              <Icon as={FaUser} />
              <Text fontWeight="semibold">{t("events.details.employee")}</Text>
              <Text>
                {employeeUser?.name} {employeeUser?.surname}
              </Text>
            </HStack>
          )}
        </VStack>

        <Separator />

        {event.completedAt && (
          <>
            <VStack align="start" gap={4}>
              <HStack>
                <Icon as={GrCompliance} />
                <Text fontWeight="semibold">
                  {t("events.details.completedAt")}
                </Text>
                <Text>{new Date(event.completedAt).toLocaleString()}</Text>
              </HStack>
              <HStack>
                <Icon as={FaUserCheck} />
                <Text fontWeight="semibold">
                  {t("events.details.completedEmployee")}
                </Text>
                <Text>
                  {endUser?.name} {endUser?.surname}
                </Text>
              </HStack>
            </VStack>
            <Separator />
          </>
        )}

        <Box>
          <Heading size="lg" mb={2}>
            {t("events.details.description")}
          </Heading>
          {event.description ? (
            <Text>{event.description}</Text>
          ) : (
            <Text color={"gray.400"}>
              {t("events.details.emptyDescription")}
            </Text>
          )}
        </Box>

        <Separator />

        <VStack align="start" color="gray.500" fontSize="sm">
          <Flex w="100%" justifyContent={"space-between"}>
            <Text>
              {t("events.details.created")}:{" "}
              {new Date(event.createdAt).toLocaleString()}
            </Text>
            <Text>
              {t("events.details.updated")}:{" "}
              {new Date(event.updatedAt).toLocaleString()}
            </Text>
          </Flex>
        </VStack>
      </Stack>
    </Box>
  );
};
