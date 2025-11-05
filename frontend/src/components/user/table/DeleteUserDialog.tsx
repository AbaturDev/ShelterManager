import { useMutation, useQueryClient } from "@tanstack/react-query";
import { useTranslation } from "react-i18next";
import { DeleteDialog } from "../../commons";
import { UserService } from "../../../api/services/user-service";
import { toaster } from "../../ui/toaster";

interface DeleteUserDialogProps {
  id: string;
  isOpen: boolean;
  onClose: () => void;
  onSuccess: () => void;
}

export const DeleteUserDialog = ({
  id,
  isOpen,
  onClose,
  onSuccess,
}: DeleteUserDialogProps) => {
  const { t } = useTranslation();
  const queryClient = useQueryClient();

  const mutation = useMutation({
    mutationFn: () => UserService.deleteUser(id),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["users"] });
      toaster.create({
        type: "success",
        title: t("success"),
        description: t("user.list.deleteToast.success"),
        closable: true,
      });
      onSuccess();
    },
    onError: () => {
      toaster.create({
        type: "error",
        title: t("error"),
        description: t("user.list.deleteToast.error"),
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
