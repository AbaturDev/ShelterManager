import { useMutation, useQueryClient } from "@tanstack/react-query";
import { DeleteDialog } from "../../../commons";
import { AnimalService } from "../../../../api/services/animals-service";
import { useTranslation } from "react-i18next";
import { toaster } from "../../../ui/toaster";

interface DeleteAnimalFileDialogProps {
  isOpen: boolean;
  id: string;
  fileName: string;
  onSuccess: () => void;
  onClose: () => void;
}

export const DeleteAnimalFileDialog = ({
  isOpen,
  id,
  fileName,
  onSuccess,
  onClose,
}: DeleteAnimalFileDialogProps) => {
  const { t } = useTranslation();
  const queryClient = useQueryClient();

  const mutation = useMutation({
    mutationFn: () => AnimalService.deleteAnimalFile(id, fileName),
    onSuccess: () => {
      queryClient.invalidateQueries({
        queryKey: ["animals", id, "files"],
      });
      toaster.create({
        type: "success",
        title: t("success"),
        description: t("animals.details.files.toast.delete.success"),
        closable: true,
      });
      onSuccess();
    },
    onError: () => {
      toaster.create({
        type: "error",
        title: t("error"),
        description: t("animals.details.files.toast.delete.error"),
        closable: true,
      });
      onClose();
    },
  });

  const handleSubmit = async () => await mutation.mutateAsync();

  return (
    <DeleteDialog isOpen={isOpen} onClose={onClose} onConfirm={handleSubmit} />
  );
};
