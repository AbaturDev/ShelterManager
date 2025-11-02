import { useMutation, useQueryClient } from "@tanstack/react-query";
import { toaster } from "../ui/toaster";
import { useTranslation } from "react-i18next";
import { DailyTasksService } from "../../api/services/daily-tasks-service";
import { Button, CloseButton, Dialog, HStack, Portal } from "@chakra-ui/react";

interface Props {
  animalId: string;
  id: string;
  date: string;
  isOpen: boolean;
  onClose: () => void;
  onSuccess: () => void;
}

export const EndDailyTaskEntryDialog = ({
  animalId,
  id,
  date,
  isOpen,
  onClose,
  onSuccess,
}: Props) => {
  const { t } = useTranslation();
  const queryClient = useQueryClient();

  const mutation = useMutation({
    mutationFn: () => DailyTasksService.endDailyTaskEntry(animalId, id),
    onSuccess: () => {
      queryClient.invalidateQueries({
        queryKey: [animalId, "daily-task", date],
      });
      toaster.create({
        type: "success",
        title: t("success"),
        description: t("dailyTasks.end.toast.success"),
        closable: true,
      });
      onSuccess();
    },
    onError: () => {
      toaster.create({
        type: "error",
        title: t("error"),
        description: t("dailyTasks.end.toast.error"),
        closable: true,
      });
      onClose();
    },
  });

  const handleConfirm = async () => await mutation.mutateAsync();

  return (
    <Portal>
      <Dialog.Root
        open={isOpen}
        onOpenChange={onClose}
        motionPreset="slide-in-bottom"
      >
        <Dialog.Backdrop />
        <Dialog.Positioner>
          <Dialog.Content>
            <Dialog.Header>
              <Dialog.Title>{t("dailyTasks.end.title")}</Dialog.Title>
              <Dialog.CloseTrigger asChild>
                <CloseButton size="sm" />
              </Dialog.CloseTrigger>
            </Dialog.Header>
            <Dialog.Body>{t("dailyTasks.end.body")}</Dialog.Body>
            <Dialog.Footer>
              <HStack justify="space-between" w="100%">
                <Button onClick={onClose} variant={"outline"}>
                  {t("dailyTasks.end.cancel")}
                </Button>
                <Button onClick={handleConfirm} background={"green.400"}>
                  {t("dailyTasks.end.confirm")}
                </Button>
              </HStack>
            </Dialog.Footer>
          </Dialog.Content>
        </Dialog.Positioner>
      </Dialog.Root>
    </Portal>
  );
};
