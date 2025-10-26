import { useMutation, useQueryClient } from "@tanstack/react-query";
import { SpeciesService } from "../../api/services/species-service";
import { toaster } from "../ui/toaster";
import { useTranslation } from "react-i18next";
import { DeleteDialog } from "../commons";

interface DeleteSpeciesDialogProps {
  id: string;
  isOpen: boolean;
  onClose: () => void;
  onSuccess: () => void;
}

export const DeleteSpeciesDialog = ({
  id,
  isOpen,
  onClose,
  onSuccess,
}: DeleteSpeciesDialogProps) => {
  const { t } = useTranslation();
  const queryClient = useQueryClient();

  const mutation = useMutation({
    mutationFn: () => SpeciesService.deleteSpecies(id),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["species"] });
      toaster.create({
        type: "success",
        title: t("success"),
        description: t("species.deleteToast.success"),
        closable: true,
      });
      onSuccess();
    },
    onError: () => {
      toaster.create({
        type: "error",
        title: t("error"),
        description: t("species.deleteToast.error"),
        closable: true,
      });
      onClose();
    },
  });

  const handleConfirm = async () => await mutation.mutateAsync();

  return (
    <DeleteDialog isOpen={isOpen} onClose={onClose} onConfirm={handleConfirm} />
  );
};
