import { zodResolver } from "@hookform/resolvers/zod";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { useState } from "react";
import { useForm } from "react-hook-form";
import z from "zod";
import { useTranslation } from "react-i18next";
import {
  Badge,
  Box,
  Button,
  CloseButton,
  Dialog,
  Field,
  Flex,
  Input,
  NativeSelect,
  Portal,
  Text,
  Textarea,
} from "@chakra-ui/react";
import { getFormErrorMessage, setFormErrorMessage } from "../../../utils";
import { EventsService } from "../../../api/services/events-service";
import { toaster } from "../../ui/toaster";
import { useUsersQuery } from "../../../hooks/useUsersQuery";
import { useAnimalsListQuery } from "../../../hooks/useAnimalsListQuery";
import type { Money } from "../../../models/event";

const schema = z.object({
  title: z
    .string()
    .min(3, setFormErrorMessage("events.fields.errors.title.min"))
    .max(40, setFormErrorMessage("events.fields.errors.title.max")),
  description: z
    .string()
    .max(250, setFormErrorMessage("events.fields.errors.description"))
    .nullable(),
  date: z
    .date(setFormErrorMessage("events.fields.errors.date"))
    .min(new Date(), setFormErrorMessage("events.fields.errors.dateFuture")),
  location: z
    .string()
    .min(3, setFormErrorMessage("events.fields.errors.location.min"))
    .max(100, setFormErrorMessage("events.fields.errors.location.max")),
  userId: z.string(setFormErrorMessage("events.fields.errors.user")),
  animalId: z.string(setFormErrorMessage("events.fields.errors.animal")),
  costAmount: z
    .number()
    .min(0, setFormErrorMessage("events.fields.errors.cost"))
    .nullable()
    .optional(),
  costCurrencyCode: z.string().nullable().optional(),
});

type FormData = z.infer<typeof schema>;

export const AddEventDialog = () => {
  const queryClient = useQueryClient();
  const { t } = useTranslation();
  const [open, setOpen] = useState(false);
  const {
    register,
    formState: { errors, isSubmitting },
    handleSubmit,
    reset,
    setValue,
  } = useForm<FormData>({
    resolver: zodResolver(schema),
    defaultValues: {
      costCurrencyCode: "PLN",
    },
  });

  const { data: usersData } = useUsersQuery({ page: 1, pageSize: 10000000 });
  const { data: animalsData } = useAnimalsListQuery({
    page: 1,
    pageSize: 10000000,
  });

  const mutation = useMutation({
    mutationFn: (data: FormData) => {
      let cost: Money | undefined = undefined;

      if (data.costAmount && data.costCurrencyCode) {
        cost = {
          amount: data.costAmount,
          currencyCode: data.costCurrencyCode,
        };
      }

      return EventsService.createEvent({
        title: data.title,
        date: data.date.toISOString(),
        animalId: data.animalId,
        location: data.location,
        userId: data.userId,
        description: data.description ?? undefined,
        cost: cost,
      });
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["events"] });
      toaster.create({
        type: "success",
        title: t("success"),
        description: t("events.create.toast.success"),
        closable: true,
      });
      setOpen(false);
      reset();
    },
    onError: () => {
      toaster.create({
        type: "error",
        title: t("error"),
        description: t("events.create.toast.error"),
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
      size={"xl"}
      placement="top"
      motionPreset="slide-in-bottom"
    >
      <Dialog.Trigger asChild>
        <Button size="md" background={"green.400"}>
          {t("events.create.button")}
        </Button>
      </Dialog.Trigger>
      <Portal>
        <Dialog.Backdrop />
        <Dialog.Positioner>
          <Dialog.Content>
            <Dialog.Header>
              <Dialog.Title>{t("events.create.dialogTitle")}</Dialog.Title>
              <Dialog.CloseTrigger asChild>
                <CloseButton size="sm" />
              </Dialog.CloseTrigger>
            </Dialog.Header>
            <form onSubmit={handleSubmit(onSubmit)}>
              <Dialog.Body>
                <Box display="flex" flexDirection="column" gap={4} p={4}>
                  <Field.Root>
                    <Field.Label>{t("events.fields.title")}</Field.Label>
                    <Input
                      variant={"outline"}
                      placeholder={t("events.fields.titlePlaceholder")}
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
                      {t("events.fields.description")}
                      <Field.RequiredIndicator
                        fallback={
                          <Badge size="xs" variant="surface">
                            {t("optional")}
                          </Badge>
                        }
                      />
                    </Field.Label>
                    <Textarea
                      placeholder={t("events.fields.description")}
                      {...register("description", {
                        setValueAs: (v) =>
                          v === "" || v === null ? null : v.toString(),
                      })}
                    />
                    {errors.description && (
                      <Text color={"red"}>
                        {getFormErrorMessage(errors.description?.message)}
                      </Text>
                    )}
                  </Field.Root>
                  <Field.Root>
                    <Field.Label>
                      {t("events.fields.date")}
                      <Field.RequiredIndicator />
                    </Field.Label>
                    <Input
                      type="date"
                      {...register("date", {
                        valueAsDate: true,
                      })}
                    />
                    {errors.date && (
                      <Text color={"red"}>
                        {getFormErrorMessage(errors.date.message)}
                      </Text>
                    )}
                  </Field.Root>
                  <Field.Root>
                    <Field.Label>{t("events.fields.location")}</Field.Label>
                    <Input
                      variant={"outline"}
                      placeholder={t("events.fields.locationPlaceholder")}
                      {...register("location")}
                    />
                    {errors.location && (
                      <Text color={"red"}>
                        {getFormErrorMessage(errors.location.message)}
                      </Text>
                    )}
                  </Field.Root>

                  <Field.Root>
                    <Field.Label>
                      {t("events.fields.animal")}
                      <Field.RequiredIndicator />
                    </Field.Label>
                    <NativeSelect.Root>
                      <NativeSelect.Field
                        onChange={(e) => setValue("animalId", e.target.value)}
                      >
                        <option value="">
                          {t("events.fields.selectAnimal")}
                        </option>
                        {animalsData?.items.map((a) => (
                          <option key={a.id} value={a.id}>
                            {a.name}
                          </option>
                        ))}
                      </NativeSelect.Field>
                      <NativeSelect.Indicator />
                    </NativeSelect.Root>
                    {errors.animalId && (
                      <Text color="red">
                        {getFormErrorMessage(errors.animalId?.message)}
                      </Text>
                    )}
                  </Field.Root>

                  <Field.Root>
                    <Field.Label>
                      {t("events.fields.user")}
                      <Field.RequiredIndicator />
                    </Field.Label>
                    <NativeSelect.Root>
                      <NativeSelect.Field
                        onChange={(e) => setValue("userId", e.target.value)}
                      >
                        <option value="">
                          {t("events.fields.selectUser")}
                        </option>
                        {usersData?.items.map((u) => (
                          <option key={u.id} value={u.id}>
                            {u.name} {u.surname}
                          </option>
                        ))}
                      </NativeSelect.Field>
                      <NativeSelect.Indicator />
                    </NativeSelect.Root>
                    {errors.userId && (
                      <Text color="red">
                        {getFormErrorMessage(errors.userId?.message)}
                      </Text>
                    )}
                  </Field.Root>
                  <Field.Root>
                    <Field.Label>
                      {t("events.fields.cost")}
                      <Field.RequiredIndicator
                        fallback={
                          <Badge size="xs" variant="surface">
                            {t("optional")}
                          </Badge>
                        }
                      />
                    </Field.Label>

                    <Flex align="center" w="100%" gap={2}>
                      <Input
                        w="80%"
                        type="number"
                        min="0"
                        step="0.01"
                        placeholder={t("events.fields.costPlaceholder")}
                        {...register("costAmount", {
                          setValueAs: (v) =>
                            v === "" || v === null ? undefined : Number(v),
                        })}
                      />
                      <NativeSelect.Root w="20%">
                        <NativeSelect.Field
                          {...register("costCurrencyCode")}
                          defaultValue="PLN"
                        >
                          <option value="PLN">PLN</option>
                          <option value="EUR">EUR</option>
                          <option value="USD">USD</option>
                        </NativeSelect.Field>
                        <NativeSelect.Indicator />
                      </NativeSelect.Root>
                    </Flex>

                    {errors.costAmount && (
                      <Text color="red">
                        {getFormErrorMessage(errors.costAmount.message)}
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
