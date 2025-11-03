import {
  Box,
  Flex,
  VStack,
  Text,
  Button,
  Icon,
  Heading,
} from "@chakra-ui/react";
import { useTranslation } from "react-i18next";
import { useDefaultTasksQuery } from "../../hooks/useDefaultTasksQuery";
import { Loading } from "../commons";
import { MdDelete, MdEdit } from "react-icons/md";
import { AddDefaultTaskDialog } from "./AddDefaultTaskDialog";
import { useState } from "react";
import { DeleteDefaultTaskDialog } from "./DeleteDefaultTaskDialog";
import { type DefaultDailyTaskEntry } from "../../models/daily-task";
import { EditDefaultTaskDialog } from "./EditDefaultTaskDialog";

interface DefaultTasksTabProps {
  animalId: string;
}

export const DefaultTasksTab = ({ animalId }: DefaultTasksTabProps) => {
  const { t } = useTranslation();
  const [isDeleteOpen, setIsDeleteOpen] = useState(false);
  const [deleteTaskId, setDeleteTaskId] = useState<string | null>(null);
  const [isEditOpen, setIsEditOpen] = useState(false);
  const [selectedTask, setSelectedTask] =
    useState<DefaultDailyTaskEntry | null>(null);

  const { data, isLoading, error } = useDefaultTasksQuery(animalId);

  if (isLoading) return <Loading />;
  if (data === undefined || error)
    return <Text color={"red"}>{t("defaultTasks.error")}</Text>;

  return (
    <>
      <Flex
        justifyContent={"space-between"}
        align={"center"}
        w="100%"
        marginY={1}
      >
        <Heading>{t("defaultTasks.title")}</Heading>
        <AddDefaultTaskDialog animalId={animalId} />
      </Flex>
      {data.length === 0 ? (
        <Text>{t("defaultTasks.empty")}</Text>
      ) : (
        <VStack align="stretch" marginTop={5}>
          {data.map((task) => (
            <Box
              key={task.id}
              p={4}
              borderWidth="1px"
              borderRadius="md"
              _hover={{ bg: "gray.50" }}
            >
              <Flex justify="space-between" align="center">
                <Box flex={1} maxW="33.3%">
                  <Text fontWeight="semibold">{task.title}</Text>
                  {task.description && (
                    <Text fontSize="sm" color="gray.600">
                      {task.description}
                    </Text>
                  )}
                </Box>
                <Box flex={1} textAlign="center" maxW="33.3%" />
                <Flex flex={1} justify="flex-end" gap={2} maxW="33.3%">
                  <Button
                    size="sm"
                    colorPalette={"blue"}
                    onClick={() => {
                      setIsEditOpen(true);
                      setSelectedTask(task);
                    }}
                  >
                    <Icon as={MdEdit} />
                    {t("edit")}
                  </Button>
                  <Button
                    size="sm"
                    colorPalette={"red"}
                    onClick={() => {
                      setIsDeleteOpen(true);
                      setDeleteTaskId(task.id);
                    }}
                  >
                    <Icon as={MdDelete} />
                    {t("delete")}
                  </Button>
                </Flex>
              </Flex>
            </Box>
          ))}
        </VStack>
      )}
      {deleteTaskId && (
        <DeleteDefaultTaskDialog
          animalId={animalId}
          id={deleteTaskId}
          isOpen={isDeleteOpen}
          onClose={() => {
            setIsDeleteOpen(false);
            setDeleteTaskId(null);
          }}
          onSuccess={() => {
            setIsDeleteOpen(false);
            setDeleteTaskId(null);
          }}
        />
      )}
      {selectedTask && (
        <EditDefaultTaskDialog
          animalId={animalId}
          task={selectedTask}
          isOpen={isEditOpen}
          onClose={() => {
            setIsEditOpen(false);
            setSelectedTask(null);
          }}
          onSuccess={() => {
            setIsEditOpen(false);
            setSelectedTask(null);
          }}
        />
      )}
    </>
  );
};
