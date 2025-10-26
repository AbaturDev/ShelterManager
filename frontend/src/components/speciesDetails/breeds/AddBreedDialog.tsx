import { zodResolver } from "@hookform/resolvers/zod";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { useState } from "react";
import { useForm } from "react-hook-form";
import z from "zod";
import { useTranslation } from "react-i18next";
import {
  Button,
  CloseButton,
  Dialog,
  Field,
  Flex,
  Input,
  Portal,
  Text,
} from "@chakra-ui/react";
import {
  getFormErrorMessage,
  setFormErrorMessage,
} from "../../../utils/form-error-handlers";
import { BreedService } from "../../../api/services/breed-service";
import { toaster } from "../../ui/toaster";
import { AxiosError } from "axios";

const schema = z.object({
  name: z
    .string()
    .min(3, setFormErrorMessage("breeds.create.errors.min"))
    .max(30, setFormErrorMessage("breeds.create.errors.max")),
});

type FormData = z.infer<typeof schema>;

interface AddBreedDialogProps {
  speciesId: string;
}

export const AddBreedDialog = ({ speciesId }: AddBreedDialogProps) => {
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
      BreedService.createBreed(speciesId, { name: data.name }),
    onSuccess: () => {
      queryClient.invalidateQueries({
        queryKey: [`species/${speciesId}/breeds`],
      });
      toaster.create({
        type: "success",
        title: t("success"),
        description: t("breeds.create.toast.success"),
        closable: true,
      });
      setOpen(false);
      reset();
    },
    onError: (err) => {
      let message: string | null = null;
      if (err instanceof AxiosError && err.status === 400) {
        message = t("breeds.create.toast.exsists");
      } else {
        message = t("breeds.create.toast.error");
      }
      toaster.create({
        type: "error",
        title: t("error"),
        description: message,
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
      motionPreset="slide-in-bottom"
    >
      <Dialog.Trigger asChild>
        <Button size="sm" background={"green.400"}>
          {t("breeds.button")}
        </Button>
      </Dialog.Trigger>
      <Portal>
        <Dialog.Backdrop />
        <Dialog.Positioner>
          <Dialog.Content>
            <Dialog.Header>
              <Dialog.Title>{t("breeds.create.dialog.title")}</Dialog.Title>
              <Dialog.CloseTrigger asChild>
                <CloseButton size="sm" />
              </Dialog.CloseTrigger>
            </Dialog.Header>
            <form onSubmit={handleSubmit(onSubmit)}>
              <Dialog.Body>
                <Field.Root>
                  <Field.Label>{t("breeds.create.dialog.name")}</Field.Label>
                  <Input
                    variant={"outline"}
                    placeholder={t("breeds.create.dialog.namePlaceholder")}
                    {...register("name")}
                  />
                  {errors.name && (
                    <Text color={"red"}>
                      {getFormErrorMessage(errors.name.message)}
                    </Text>
                  )}
                </Field.Root>
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
