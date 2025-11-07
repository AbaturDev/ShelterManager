import { useMutation, useQueryClient } from "@tanstack/react-query";
import { toaster } from "../ui/toaster";
import { useTranslation } from "react-i18next";
import { Button, CloseButton, Dialog, HStack, Portal } from "@chakra-ui/react";
import { EventsService } from "../../api/services/events-service";

interface Props {
  id: string;
  isOpen: boolean;
  onClose: () => void;
  onSuccess: () => void;
}

export const EndEventDialog = ({ id, isOpen, onClose, onSuccess }: Props) => {
  const { t } = useTranslation();
  const queryClient = useQueryClient();

  const mutation = useMutation({
    mutationFn: () => EventsService.endEvent(id),
    onSuccess: () => {
      queryClient.invalidateQueries({
        queryKey: ["events"],
      });
      toaster.create({
        type: "success",
        title: t("success"),
        description: t("events.end.toast.success"),
        closable: true,
      });
      onSuccess();
    },
    onError: () => {
      toaster.create({
        type: "error",
        title: t("error"),
        description: t("events.end.toast.error"),
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
              <Dialog.Title>{t("events.end.title")}</Dialog.Title>
              <Dialog.CloseTrigger asChild>
                <CloseButton size="sm" />
              </Dialog.CloseTrigger>
            </Dialog.Header>
            <Dialog.Body>{t("events.end.body")}</Dialog.Body>
            <Dialog.Footer>
              <HStack justify="space-between" w="100%">
                <Button onClick={onClose} variant={"outline"}>
                  {t("cancel")}
                </Button>
                <Button
                  onClick={handleConfirm}
                  background={"green.400"}
                  loading={mutation.isPending}
                >
                  {t("confirm")}
                </Button>
              </HStack>
            </Dialog.Footer>
          </Dialog.Content>
        </Dialog.Positioner>
      </Dialog.Root>
    </Portal>
  );
};
