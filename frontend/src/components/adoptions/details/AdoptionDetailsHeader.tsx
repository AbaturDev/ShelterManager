import { Button, Flex, HStack, Icon } from "@chakra-ui/react";
import { BackButton } from "../../commons";
import { MdDelete, MdEdit } from "react-icons/md";
import { useTranslation } from "react-i18next";
import { useState } from "react";
import { useNavigate } from "react-router-dom";
import type { AdoptionDetails } from "../../../models/adoptions";
import { DeleteAdoptionDialog } from "../DeleteAdoptionDialog";

interface Props {
  adoption: AdoptionDetails | undefined;
  isLoading: boolean;
}

export const AdoptionDetailsHeader = ({ adoption, isLoading }: Props) => {
  const { t } = useTranslation();
  const navigate = useNavigate();

  const [isDeleteOpen, setIsDeleteOpen] = useState(false);
  const [isEditOpen, setIsEditOpen] = useState(false);

  return (
    <Flex
      w="100%"
      justifyContent={"space-between"}
      alignItems={"center"}
      marginBottom={5}
    >
      <BackButton />
      {!isLoading && adoption !== undefined && (
        <>
          <HStack gap={5}>
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
          <DeleteAdoptionDialog
            id={adoption.id}
            isOpen={isDeleteOpen}
            onClose={() => {
              setIsDeleteOpen(false);
            }}
            onSuccess={() => {
              setIsDeleteOpen(false);
              navigate("/adoptions");
            }}
          />
          {/*
          <EditEventDialog
            event={event}
            isOpen={isEditOpen}
            onClose={() => {
              setIsEditOpen(false);
            }}
            onSuccess={() => {
              setIsEditOpen(false);
            }}
          /> */}
          {isEditOpen}
        </>
      )}
    </Flex>
  );
};
