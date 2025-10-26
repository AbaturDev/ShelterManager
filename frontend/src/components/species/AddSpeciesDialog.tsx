import { zodResolver } from "@hookform/resolvers/zod";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { useState } from "react";
import { useForm } from "react-hook-form";
import z from "zod";
import { SpeciesService } from "../../api/services/species-service";
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
import { toaster } from "../ui/toaster";
import { AxiosError } from "axios";
import {
  getFormErrorMessage,
  setFormErrorMessage,
} from "../../utils/form-error-handlers";

const schema = z.object({
  name: z
    .string()
    .min(3, setFormErrorMessage("species.create.errors.min"))
    .max(30, setFormErrorMessage("species.create.errors.max")),
});

type FormData = z.infer<typeof schema>;

export const AddSpeciesDialog = () => {
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
      SpeciesService.createSpecies({ name: data.name }),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["species"] });
      toaster.create({
        type: "success",
        title: t("success"),
        description: t("species.create.toast.success"),
        closable: true,
      });
      setOpen(false);
      reset();
    },
    onError: (err) => {
      let message: string | null = null;
      if (err instanceof AxiosError && err.status === 400) {
        message = t("species.create.toast.exsists");
      } else {
        message = t("species.create.toast.error");
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
        <Button size="lg" background={"green.400"}>
          {t("species.create.button")}
        </Button>
      </Dialog.Trigger>
      <Portal>
        <Dialog.Backdrop />
        <Dialog.Positioner>
          <Dialog.Content>
            <Dialog.Header>
              <Dialog.Title>{t("species.create.dialogTitle")}</Dialog.Title>
              <Dialog.CloseTrigger asChild>
                <CloseButton size="sm" />
              </Dialog.CloseTrigger>
            </Dialog.Header>
            <form onSubmit={handleSubmit(onSubmit)}>
              <Dialog.Body>
                <Field.Root>
                  <Field.Label>{t("species.create.name")}</Field.Label>
                  <Input
                    variant={"outline"}
                    placeholder={t("species.create.namePlaceholder")}
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
