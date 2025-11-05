import { useMutation, useQueryClient } from "@tanstack/react-query";
import { DeleteDialog } from "../../commons";
import { useTranslation } from "react-i18next";
import { BreedService } from "../../../api/services/breed-service";
import { toaster } from "../../ui/toaster";

interface DeleteBreedDialogProps {
  id: string;
  speciesId: string;
  isOpen: boolean;
  onClose: () => void;
  onSuccess: () => void;
}

export const DeleteBreedDialog = ({
  id,
  speciesId,
  isOpen,
  onClose,
  onSuccess,
}: DeleteBreedDialogProps) => {
  const { t } = useTranslation();
  const queryClient = useQueryClient();

  const mutation = useMutation({
    mutationFn: () => BreedService.deleteBreed(id, speciesId),
    onSuccess: () => {
      queryClient.invalidateQueries({
        queryKey: [`species/${speciesId}/breeds`],
      });
      toaster.create({
        type: "success",
        title: t("success"),
        description: t("breeds.delete.toast.success"),
        closable: true,
      });
      onSuccess();
    },
    onError: () => {
      toaster.create({
        type: "error",
        title: t("error"),
        description: t("breeds.delete.toast.error"),
        closable: true,
      });
      onClose();
    },
  });

  const handleConfirm = async () => await mutation.mutateAsync();

  return (
    <DeleteDialog
      isOpen={isOpen}
      onClose={onClose}
      onConfirm={handleConfirm}
      isLoading={mutation.isPending}
    />
  );
};
