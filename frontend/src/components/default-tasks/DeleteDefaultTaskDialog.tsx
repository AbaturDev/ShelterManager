import { useMutation, useQueryClient } from "@tanstack/react-query";
import { toaster } from "../ui/toaster";
import { useTranslation } from "react-i18next";
import { DeleteDialog } from "../commons";
import { DailyTasksService } from "../../api/services/daily-tasks-service";

interface Props {
  animalId: string;
  id: string;
  isOpen: boolean;
  onClose: () => void;
  onSuccess: () => void;
}

export const DeleteDefaultTaskDialog = ({
  animalId,
  id,
  isOpen,
  onClose,
  onSuccess,
}: Props) => {
  const { t } = useTranslation();
  const queryClient = useQueryClient();

  const mutation = useMutation({
    mutationFn: () =>
      DailyTasksService.deleteDefaultDailyTaskEntry(animalId, id),
    onSuccess: () => {
      queryClient.invalidateQueries({
        queryKey: ["default-tasks", animalId],
      });
      toaster.create({
        type: "success",
        title: t("success"),
        description: t("defaultTasks.toast.delete.success"),
        closable: true,
      });
      onSuccess();
    },
    onError: () => {
      toaster.create({
        type: "error",
        title: t("error"),
        description: t("defaultTasks.toast.delete.error"),
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
