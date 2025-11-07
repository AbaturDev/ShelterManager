import { Button, Dialog, HStack, Portal } from "@chakra-ui/react";
import { useTranslation } from "react-i18next";
import { MdDelete } from "react-icons/md";

interface DeleteDialogProps {
  isOpen: boolean;
  isLoading?: boolean;
  onClose: () => void;
  onConfirm: () => void;
}

export const DeleteDialog = ({
  isOpen,
  isLoading,
  onClose,
  onConfirm,
}: DeleteDialogProps) => {
  const { t } = useTranslation();

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
              <Dialog.Title>{t("deleteDialog.title")}</Dialog.Title>
            </Dialog.Header>
            <Dialog.Body>{t("deleteDialog.body")}</Dialog.Body>
            <Dialog.Footer>
              <HStack justify="space-between" w="100%">
                <Button onClick={onClose} variant={"outline"}>
                  {t("deleteDialog.cancel")}
                </Button>
                <Button
                  onClick={onConfirm}
                  colorPalette={"red"}
                  loading={isLoading}
                >
                  {t("deleteDialog.confirm")}
                  <MdDelete />
                </Button>
              </HStack>
            </Dialog.Footer>
          </Dialog.Content>
        </Dialog.Positioner>
      </Dialog.Root>
    </Portal>
  );
};
