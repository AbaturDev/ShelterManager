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
  NativeSelect,
  Portal,
  Text,
} from "@chakra-ui/react";
import {
  getFormErrorMessage,
  setFormErrorMessage,
} from "../../../utils/form-error-handlers";
import { toaster } from "../../ui/toaster";
import { AccountService } from "../../../api/services/account-service";
import type { Role } from "../../../models/account";
import { AxiosError } from "axios";

const schema = z.object({
  name: z
    .string()
    .min(3, setFormErrorMessage("user.register.error.nameMin"))
    .max(30, setFormErrorMessage("user.register.error.surnameMax")),
  surname: z
    .string()
    .min(3, setFormErrorMessage("user.register.error.surnameMin"))
    .max(30, setFormErrorMessage("user.register.error.surnameMax")),
  email: z.email(setFormErrorMessage("user.register.error.email")),
  role: z.enum(["User", "Admin"]),
});

type FormData = z.infer<typeof schema>;

export const RegisterUserDialog = () => {
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
      AccountService.register({
        name: data.name,
        surname: data.surname,
        email: data.email,
        role: data.role as Role,
      }),
    onSuccess: () => {
      queryClient.invalidateQueries({
        queryKey: [`users`],
      });
      toaster.create({
        type: "success",
        title: t("success"),
        description: t("user.register.toast.success"),
        closable: true,
      });
      setOpen(false);
      reset();
    },
    onError: (err) => {
      if (
        err instanceof AxiosError &&
        err.response?.data?.detail === "Email is already taken"
      ) {
        toaster.create({
          type: "error",
          title: t("success"),
          description: t("user.register.toast.emailTaken"),
          closable: true,
        });
      } else {
        toaster.create({
          type: "error",
          title: t("success"),
          description: t("user.register.toast.error"),
          closable: true,
        });
      }
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
          {t("user.register.button")}
        </Button>
      </Dialog.Trigger>
      <Portal>
        <Dialog.Backdrop />
        <Dialog.Positioner>
          <Dialog.Content>
            <Dialog.Header>
              <Dialog.Title>{t("user.register.title")}</Dialog.Title>
              <Dialog.CloseTrigger asChild>
                <CloseButton size="sm" />
              </Dialog.CloseTrigger>
            </Dialog.Header>
            <form onSubmit={handleSubmit(onSubmit)}>
              <Dialog.Body spaceY={8} marginBottom={3}>
                <Field.Root>
                  <Field.Label>{t("user.register.name")}</Field.Label>
                  <Input
                    variant={"outline"}
                    placeholder={t("user.register.namePlaceholder")}
                    {...register("name")}
                  />
                  {errors.name && (
                    <Text color={"red"}>
                      {getFormErrorMessage(errors.name.message)}
                    </Text>
                  )}
                </Field.Root>
                <Field.Root>
                  <Field.Label>{t("user.register.surname")}</Field.Label>
                  <Input
                    variant={"outline"}
                    placeholder={t("user.register.surnamePlaceholder")}
                    {...register("surname")}
                  />
                  {errors.surname && (
                    <Text color={"red"}>
                      {getFormErrorMessage(errors.surname.message)}
                    </Text>
                  )}
                </Field.Root>
                <Field.Root>
                  <Field.Label>{t("user.register.email")}</Field.Label>
                  <Input
                    variant={"outline"}
                    placeholder={t("user.register.emailPlaceholder")}
                    {...register("email")}
                  />
                  {errors.email && (
                    <Text color={"red"}>
                      {getFormErrorMessage(errors.email.message)}
                    </Text>
                  )}
                </Field.Root>
                <Field.Root>
                  <Field.Label>{t("user.register.role.title")}</Field.Label>
                  <NativeSelect.Root>
                    <NativeSelect.Field {...register("role")}>
                      <option value={"User"}>
                        {t("user.register.role.user")}
                      </option>
                      <option value={"Admin"}>
                        {t("user.register.role.admin")}
                      </option>
                    </NativeSelect.Field>
                    <NativeSelect.Indicator />
                  </NativeSelect.Root>
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
