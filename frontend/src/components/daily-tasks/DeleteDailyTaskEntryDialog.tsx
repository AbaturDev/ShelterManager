import { useMutation, useQueryClient } from "@tanstack/react-query";
import { toaster } from "../ui/toaster";
import { useTranslation } from "react-i18next";
import { DeleteDialog } from "../commons";
import { DailyTasksService } from "../../api/services/daily-tasks-service";

interface Props {
  animalId: string;
  id: string;
  date: string;
  isOpen: boolean;
  onClose: () => void;
  onSuccess: () => void;
}

export const DeleteDailyTaskEntryDialog = ({
  animalId,
  id,
  date,
  isOpen,
  onClose,
  onSuccess,
}: Props) => {
  const { t } = useTranslation();
  const queryClient = useQueryClient();

  const mutation = useMutation({
    mutationFn: () => DailyTasksService.deleteDailyTaskEntry(animalId, id),
    onSuccess: () => {
      queryClient.invalidateQueries({
        queryKey: [animalId, "daily-task", date],
      });
      toaster.create({
        type: "success",
        title: t("success"),
        description: t("dailyTasks.delete.toast.success"),
        closable: true,
      });
      onSuccess();
    },
    onError: () => {
      toaster.create({
        type: "error",
        title: t("error"),
        description: t("dailyTasks.delete.toast.error"),
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
