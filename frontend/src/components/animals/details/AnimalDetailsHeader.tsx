import { Button, Flex, HStack, Icon } from "@chakra-ui/react";
import { BackButton } from "../../commons";
import { useState } from "react";
import { useTranslation } from "react-i18next";
import { DeleteAnimalDialog } from "../DeleteAnimalDialog";
import { useNavigate } from "react-router-dom";
import { MdDelete, MdEdit } from "react-icons/md";
import { EditAnimalDialog } from "../EditAnimalDialog";

interface AnimalsDetailsHeaderProps {
  id: string;
}

export const AnimalDetailsHeader = ({ id }: AnimalsDetailsHeaderProps) => {
  const { t } = useTranslation();
  const navigate = useNavigate();
  const [isDeleteOpen, setIsDeleteOpen] = useState(false);
  const [isEditOpen, setIsEditOpen] = useState(false);

  return (
    <>
      <Flex w="100%" justifyContent={"space-between"} alignItems={"center"}>
        <BackButton />
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
      </Flex>
      <DeleteAnimalDialog
        id={id}
        isOpen={isDeleteOpen}
        onClose={() => setIsDeleteOpen(false)}
        onSuccess={() => {
          navigate("/animals");
          setIsDeleteOpen(false);
        }}
      />
      <EditAnimalDialog
        id={id}
        isOpen={isEditOpen}
        onClose={() => setIsEditOpen(false)}
        onSuccess={() => setIsEditOpen(false)}
      />
    </>
  );
};
