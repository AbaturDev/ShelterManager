import { useMutation, useQueryClient } from "@tanstack/react-query";
import { toaster } from "../ui/toaster";
import { useTranslation } from "react-i18next";
import { DeleteDialog } from "../commons";
import { AnimalService } from "../../api/services/animals-service";

interface DeleteAnimalDialogProps {
  id: string;
  isOpen: boolean;
  onClose: () => void;
  onSuccess: () => void;
}

export const DeleteAnimalDialog = ({
  id,
  isOpen,
  onClose,
  onSuccess,
}: DeleteAnimalDialogProps) => {
  const { t } = useTranslation();
  const queryClient = useQueryClient();

  const mutation = useMutation({
    mutationFn: () => AnimalService.deleteAnimal(id),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["animals"] });
      toaster.create({
        type: "success",
        title: t("success"),
        description: t("animals.deleteToast.success"),
        closable: true,
      });
      onSuccess();
    },
    onError: () => {
      toaster.create({
        type: "error",
        title: t("error"),
        description: t("animals.deleteToast.error"),
        closable: true,
      });
      onClose();
    },
  });

  const handleConfirm = async () => {
    if (mutation.isPending) return;
    await mutation.mutateAsync();
  };

  return (
    <DeleteDialog isOpen={isOpen} onClose={onClose} onConfirm={handleConfirm} />
  );
};
