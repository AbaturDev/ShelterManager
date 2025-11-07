import { Button, Flex, HStack, Icon } from "@chakra-ui/react";
import { BackButton } from "../../commons";
import type { Event } from "../../../models/event";
import { MdDelete, MdEdit } from "react-icons/md";
import { useTranslation } from "react-i18next";
import { GrCompliance } from "react-icons/gr";
import { DeleteEventDialog, EditEventDialog, EndEventDialog } from "..";
import { useState } from "react";
import { useNavigate } from "react-router-dom";

interface EventDetailsHeaderProps {
  event: Event | undefined;
  isLoading: boolean;
}

export const EventDetailsHeader = ({
  event,
  isLoading,
}: EventDetailsHeaderProps) => {
  const { t } = useTranslation();
  const navigate = useNavigate();

  const [isDeleteOpen, setIsDeleteOpen] = useState(false);
  const [isEditOpen, setIsEditOpen] = useState(false);
  const [isEndOpen, setIsEndOpen] = useState(false);

  return (
    <Flex
      w="100%"
      justifyContent={"space-between"}
      alignItems={"center"}
      marginBottom={5}
    >
      <BackButton />
      {!isLoading && event !== undefined && (
        <>
          <HStack gap={5}>
            <Button
              background={"green.600"}
              disabled={event.isDone}
              onClick={() => setIsEndOpen(true)}
            >
              <Icon as={GrCompliance} />
              {t("events.details.end")}
            </Button>
            <Button
              variant={"solid"}
              colorPalette={"blue"}
              onClick={() => setIsEditOpen(true)}
            >
              <Icon as={MdEdit} />
              {t("edit")}
            </Button>
            <Button
              colorPalette={"red"}
              variant={"solid"}
              onClick={() => setIsDeleteOpen(true)}
            >
              <Icon as={MdDelete} />
              {t("delete")}
            </Button>
          </HStack>
          <DeleteEventDialog
            id={event.id}
            isOpen={isDeleteOpen}
            onClose={() => {
              setIsDeleteOpen(false);
            }}
            onSuccess={() => {
              setIsDeleteOpen(false);
              navigate("/events");
            }}
          />
          <EndEventDialog
            id={event.id}
            isOpen={isEndOpen}
            onClose={() => {
              setIsEndOpen(false);
            }}
            onSuccess={() => {
              setIsEndOpen(false);
            }}
          />
          <EditEventDialog
            event={event}
            isOpen={isEditOpen}
            onClose={() => {
              setIsEditOpen(false);
            }}
            onSuccess={() => {
              setIsEditOpen(false);
            }}
          />
        </>
      )}
    </Flex>
  );
};
