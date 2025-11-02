import {
  Box,
  Button,
  CloseButton,
  Dialog,
  Field,
  Flex,
  Input,
  Portal,
  Text,
} from "@chakra-ui/react";
import z from "zod";
import { getFormErrorMessage, setFormErrorMessage } from "../../utils";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { useTranslation } from "react-i18next";
import { useState } from "react";
import { zodResolver } from "@hookform/resolvers/zod";
import { useForm } from "react-hook-form";
import { DailyTasksService } from "../../api/services/daily-tasks-service";
import { toaster } from "../ui/toaster";

const schema = z.object({
  title: z
    .string()
    .min(3, setFormErrorMessage("dailyTasks.create.errors.title.min"))
    .max(30, setFormErrorMessage("dailyTasks.create.errors.title.max")),
  description: z
    .string()
    .max(300, setFormErrorMessage("dailyTasks.create.errors.description"))
    .nullable(),
});

type FormData = z.infer<typeof schema>;

interface Props {
  animalId: string;
  date: string;
  disabled: boolean;
}

export const AddDailyTaskEntryDialog = ({
  animalId,
  date,
  disabled,
}: Props) => {
  const queryClient = useQueryClient();
  const { t } = useTranslation();
  const [open, setOpen] = useState(false);
  const {
    register,
    formState: { errors, isSubmitting },
    handleSubmit,
    reset,
  } = useForm<FormData>({ resolver: zodResolver(schema) });

  const mutation = useMutation({
    mutationFn: (data: FormData) =>
      DailyTasksService.createDailyTaskEntry(animalId, {
        title: data.title,
        description: data.description ?? undefined,
      }),
    onSuccess: () => {
      queryClient.invalidateQueries({
        queryKey: [animalId, "daily-task", date],
      });
      toaster.create({
        type: "success",
        title: t("success"),
        description: t("dailyTasks.create.toast.success"),
        closable: true,
      });
      setOpen(false);
      reset();
    },
    onError: () => {
      toaster.create({
        type: "error",
        title: t("error"),
        description: t("dailyTasks.create.toast.error"),
        closable: true,
      });
      setOpen(false);
      reset();
    },
  });

  const onSubmit = async (data: FormData) => await mutation.mutateAsync(data);

  return (
    <Dialog.Root
      open={open}
      onOpenChange={(e) => {
        setOpen(e.open);
        reset();
      }}
      placement="top"
      size={"lg"}
      motionPreset="slide-in-bottom"
    >
      <Dialog.Trigger asChild>
        <Button size="md" background={"green.400"} disabled={disabled}>
          {t("dailyTasks.create.button")}
        </Button>
      </Dialog.Trigger>
      <Portal>
        <Dialog.Backdrop />
        <Dialog.Positioner>
          <Dialog.Content>
            <Dialog.Header>
              <Dialog.Title>{t("dailyTasks.create.dialogTitle")}</Dialog.Title>
              <Dialog.CloseTrigger asChild>
                <CloseButton size="sm" />
              </Dialog.CloseTrigger>
            </Dialog.Header>
            <form onSubmit={handleSubmit(onSubmit)}>
              <Dialog.Body>
                <Box display="flex" flexDirection="column" gap={4} p={4}>
                  <Field.Root>
                    <Field.Label>{t("dailyTasks.create.title")}</Field.Label>
                    <Input
                      variant={"outline"}
                      placeholder={t("dailyTasks.create.titlePlaceholder")}
                      {...register("title")}
                    />
                    {errors.title && (
                      <Text color={"red"}>
                        {getFormErrorMessage(errors.title.message)}
                      </Text>
                    )}
                  </Field.Root>
                  <Field.Root>
                    <Field.Label>
                      {t("dailyTasks.create.description")}
                    </Field.Label>
                    <Input
                      variant={"outline"}
                      placeholder={t(
                        "dailyTasks.create.descriptionPlaceholder"
                      )}
                      {...register("description")}
                    />
                    {errors.description && (
                      <Text color={"red"}>
                        {getFormErrorMessage(errors.description.message)}
                      </Text>
                    )}
                  </Field.Root>
                </Box>
              </Dialog.Body>
              <Dialog.Footer>
                <Flex justify="space-between" w="100%">
                  <Dialog.ActionTrigger asChild>
                    <Button variant={"outline"}>{t("cancel")}</Button>
                  </Dialog.ActionTrigger>
                  <Button
                    type="submit"
                    background={"green.400"}
                    loading={isSubmitting}
                  >
                    {t("save")}
                  </Button>
                </Flex>
              </Dialog.Footer>
            </form>
          </Dialog.Content>
        </Dialog.Positioner>
      </Portal>
    </Dialog.Root>
  );
};
