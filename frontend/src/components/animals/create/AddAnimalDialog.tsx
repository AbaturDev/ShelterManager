import { Button, CloseButton, Dialog, Heading, Portal } from "@chakra-ui/react";
import { t } from "i18next";
import { useState } from "react";
import { AddAnimalDialogContent } from "./AddAnimalDialogContent";

export const AddAnimalDialog = () => {
  const [isOpen, setIsOpen] = useState(false);

  return (
    <Dialog.Root
      open={isOpen}
      onOpenChange={(e) => {
        setIsOpen(e.open);
      }}
      placement="top"
      size={"xl"}
      motionPreset="slide-in-bottom"
    >
      <Dialog.Trigger asChild>
        <Button size="lg" background={"green.400"}>
          {t("animals.list.add")}
        </Button>
      </Dialog.Trigger>
      <Portal>
        <Dialog.Backdrop />
        <Dialog.Positioner>
          <Dialog.Content>
            <Dialog.Header justifyContent={"center"}>
              <Heading size={"2xl"}>{t("animals.create.title")}</Heading>
              <Dialog.CloseTrigger asChild>
                <CloseButton size="sm" />
              </Dialog.CloseTrigger>
            </Dialog.Header>
            <Dialog.Body>
              <AddAnimalDialogContent onClose={() => setIsOpen(false)} />
            </Dialog.Body>
          </Dialog.Content>
        </Dialog.Positioner>
      </Portal>
    </Dialog.Root>
  );
};
