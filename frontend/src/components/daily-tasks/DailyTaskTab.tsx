import { useState } from "react";
import { useDailyTaskQuery } from "../../hooks/useDailyTaskQuery";
import { Loading } from "../commons";
import { Box, Button, Flex, Icon, Text, VStack } from "@chakra-ui/react";
import { MdChevronLeft, MdChevronRight } from "react-icons/md";
import { useTranslation } from "react-i18next";
import { AddDailyTaskEntryDialog } from "./AddDailyTaskEntryDialog";
import { DeleteDailyTaskEntryDialog } from "./DeleteDailyTaskEntryDialog";
import { EndDailyTaskEntryDialog } from "./EndDailyTaskEntryDialog";

interface DailyTaskTabProps {
  animalId: string;
}

export const DailyTaskTab = ({ animalId }: DailyTaskTabProps) => {
  const { t } = useTranslation();
  const [date, setDate] = useState<string>(
    () => new Date().toISOString().split("T")[0]
  );
  const [isDeleteOpen, setIsDeleteOpen] = useState(false);
  const [deleteEntryId, setDeleteEntryId] = useState<string | null>(null);
  const [isEndOpen, setIsEndOpen] = useState(false);
  const [endEntryId, setEndEntryId] = useState<string | null>(null);

  const { data, isLoading } = useDailyTaskQuery(animalId, date);

  const changeDate = (days: number) => {
    const current = new Date(date);
    current.setDate(current.getDate() + days);
    setDate(current.toISOString().split("T")[0]);
  };

  if (isLoading) return <Loading />;

  const isToday = date === new Date().toISOString().split("T")[0];

  return (
    <>
      <Flex
        align="center"
        justify="center"
        gap={4}
        marginY={3}
        position="relative"
      >
        <Flex align="center" justify="center" gap={4}>
          <Icon
            as={MdChevronLeft}
            onClick={() => changeDate(-1)}
            cursor="pointer"
          />
          <Text fontSize="lg" fontWeight="semibold">
            {date}
          </Text>
          <Icon
            as={MdChevronRight}
            onClick={() => changeDate(1)}
            cursor="pointer"
          />
        </Flex>

        <Flex position="absolute" right={0}>
          <AddDailyTaskEntryDialog
            animalId={animalId}
            date={date}
            disabled={!isToday}
          />
        </Flex>
      </Flex>
      {data && data.entries.length > 0 ? (
        <VStack align="stretch" marginTop={5}>
          {data.entries.map((entry) => (
            <Box
              key={entry.id}
              p={4}
              borderWidth="1px"
              borderRadius="md"
              _hover={{ bg: "gray.50" }}
            >
              <Flex justify="space-between" align="center">
                <Box flex={1} maxW="33.3%">
                  <Text fontWeight="semibold">{entry.title}</Text>
                  {entry.description && (
                    <Text fontSize="sm" color="gray.600">
                      {entry.description}
                    </Text>
                  )}
                </Box>
                <Box flex={1} textAlign="center" maxW="33.3%">
                  {entry.isCompleted && (
                    <VStack>
                      <Text fontSize={"md"} font={"initial"}>
                        {t("dailyTasks.list.completed")}{" "}
                        {entry.completedAt &&
                          new Date(entry.completedAt).toLocaleTimeString()}
                      </Text>
                      {entry.userDisplayName && (
                        <Text fontSize={"md"} font={"initial"}>
                          {t("dailyTasks.list.by")} {entry.userDisplayName}
                        </Text>
                      )}
                    </VStack>
                  )}
                </Box>
                <Flex flex={1} justify="flex-end" gap={2} maxW="33.3%">
                  <Button
                    size="sm"
                    background={"green.600"}
                    disabled={entry.isCompleted || !isToday}
                    onClick={() => {
                      setEndEntryId(entry.id);
                      setIsEndOpen(true);
                    }}
                  >
                    {t("dailyTasks.list.end")}
                  </Button>
                  <Button
                    size="sm"
                    colorPalette={"red"}
                    disabled={!isToday}
                    onClick={() => {
                      setDeleteEntryId(entry.id);
                      setIsDeleteOpen(true);
                    }}
                  >
                    {t("delete")}
                  </Button>
                </Flex>
              </Flex>
            </Box>
          ))}
        </VStack>
      ) : (
        <Text textAlign="center" color="gray.500">
          {t("dailyTasks.list.empty")}
        </Text>
      )}
      {deleteEntryId && (
        <DeleteDailyTaskEntryDialog
          animalId={animalId}
          id={deleteEntryId}
          date={date}
          isOpen={isDeleteOpen}
          onClose={() => {
            setIsDeleteOpen(false);
            setDeleteEntryId(null);
          }}
          onSuccess={() => {
            setIsDeleteOpen(false);
            setDeleteEntryId(null);
          }}
        />
      )}
      {endEntryId && (
        <EndDailyTaskEntryDialog
          animalId={animalId}
          id={endEntryId}
          date={date}
          isOpen={isEndOpen}
          onClose={() => {
            setIsEndOpen(false);
            setEndEntryId(null);
          }}
          onSuccess={() => {
            setIsEndOpen(false);
            setEndEntryId(null);
          }}
        />
      )}
    </>
  );
};
