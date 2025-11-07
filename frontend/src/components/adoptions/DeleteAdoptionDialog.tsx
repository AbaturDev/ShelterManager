import { useMutation, useQueryClient } from "@tanstack/react-query";
import { toaster } from "../ui/toaster";
import { useTranslation } from "react-i18next";
import { DeleteDialog } from "../commons";
import { AdoptionService } from "../../api/services/adoption-service";

interface Props {
  id: string;
  isOpen: boolean;
  onClose: () => void;
  onSuccess: () => void;
}

export const DeleteAdoptionDialog = ({
  id,
  isOpen,
  onClose,
  onSuccess,
}: Props) => {
  const { t } = useTranslation();
  const queryClient = useQueryClient();

  const mutation = useMutation({
    mutationFn: () => AdoptionService.deleteAdoption(id),
    onSuccess: () => {
      queryClient.invalidateQueries({
        queryKey: ["adoptions"],
      });
      toaster.create({
        type: "success",
        title: t("success"),
        description: t("adoptions.delete.toast.success"),
        closable: true,
      });
      onSuccess();
    },
    onError: () => {
      toaster.create({
        type: "error",
        title: t("error"),
        description: t("adoptions.delete.toast.error"),
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
