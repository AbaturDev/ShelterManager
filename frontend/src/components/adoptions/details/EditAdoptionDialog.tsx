import {
  Badge,
  Box,
  Button,
  CloseButton,
  Dialog,
  Field,
  Flex,
  Heading,
  Input,
  NativeSelect,
  Portal,
  Separator,
  Text,
  Textarea,
} from "@chakra-ui/react";
import z from "zod";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { useTranslation } from "react-i18next";
import { zodResolver } from "@hookform/resolvers/zod";
import { useForm } from "react-hook-form";
import {
  PossibleAdoptionStatus,
  type AdoptionDetails,
  type AdoptionEvent,
  type AdoptionStatus,
} from "../../../models/adoptions";
import { getFormErrorMessage, setFormErrorMessage } from "../../../utils";
import { AdoptionService } from "../../../api/services/adoption-service";
import { toaster } from "../../ui/toaster";
import { useEffect } from "react";

const schema = z
  .object({
    status: z.number(),
    note: z
      .string()
      .max(350, setFormErrorMessage("adoptions.fields.errors.note"))
      .nullable(),

    eventTitle: z
      .string()
      .min(3, setFormErrorMessage("events.fields.errors.title.min"))
      .max(40, setFormErrorMessage("events.fields.errors.title.max"))
      .optional()
      .nullable(),
    eventDescription: z
      .string()
      .max(250, setFormErrorMessage("events.fields.errors.description"))
      .nullable()
      .optional(),
    eventDate: z
      .date(setFormErrorMessage("events.fields.errors.date"))
      .min(new Date(), setFormErrorMessage("events.fields.errors.dateFuture"))
      .optional()
      .nullable(),
    eventLocation: z
      .string()
      .min(3, setFormErrorMessage("events.fields.errors.location.min"))
      .max(100, setFormErrorMessage("events.fields.errors.location.max"))
      .optional()
      .nullable(),
  })
  .superRefine((data, ctx) => {
    if (data.status === PossibleAdoptionStatus.Approved) {
      if (!data.eventTitle || data.eventTitle.trim().length < 3) {
        ctx.addIssue({
          code: "custom",
          path: ["eventTitle"],
          message: setFormErrorMessage("events.fields.errors.title.min"),
        });
      }
      if (!data.eventDate) {
        ctx.addIssue({
          code: "custom",
          path: ["eventDate"],
          message: setFormErrorMessage("events.fields.errors.date"),
        });
      }
      if (!data.eventLocation || data.eventLocation.trim().length < 3) {
        ctx.addIssue({
          code: "custom",
          path: ["eventLocation"],
          message: setFormErrorMessage("events.fields.errors.location.min"),
        });
      }
    }
  });

type FormData = z.infer<typeof schema>;

interface Props {
  adoption: AdoptionDetails;
  isOpen: boolean;
  onClose: () => void;
  onSuccess: () => void;
}

export const EditAdoptionDialog = ({
  adoption,
  isOpen,
  onClose,
  onSuccess,
}: Props) => {
  const queryClient = useQueryClient();
  const { t } = useTranslation();
  const {
    register,
    formState: { errors, isSubmitting },
    handleSubmit,
    reset,
    watch,
    setValue,
  } = useForm<FormData>({
    resolver: zodResolver(schema),
    defaultValues: {
      status: adoption?.status,
      note: adoption.note,
    },
  });

  const mutation = useMutation({
    mutationFn: (data: FormData) => {
      let event: AdoptionEvent | undefined;
      if (data.status === PossibleAdoptionStatus.Approved) {
        event = {
          title: data.eventTitle!,
          description: data.eventDescription ?? undefined,
          location: data.eventLocation!,
          plannedAdoptionDate: data.eventDate!.toISOString(),
        };
      }

      return AdoptionService.updateAdoption(adoption.id, {
        status: data.status as AdoptionStatus,
        note: data.note ?? undefined,
        event: event,
      });
    },
    onSuccess: () => {
      queryClient.invalidateQueries({
        queryKey: ["adoptions"],
      });

      let message: string;
      if (watch("status") === PossibleAdoptionStatus.Approved) {
        message = t("adoptions.edit.toast.successWithEvent");
      } else {
        message = t("adoptions.edit.toast.success");
      }

      toaster.create({
        type: "success",
        title: t("success"),
        description: message,
        closable: true,
      });
      onSuccess();
      reset();
    },
    onError: () => {
      toaster.create({
        type: "error",
        title: t("error"),
        description: t("adoptions.edit.toast.error"),
        closable: true,
      });
      onClose();
      reset();
    },
  });

  const onSubmit = async (data: FormData) => await mutation.mutateAsync(data);

  const currStatus = watch("status");

  useEffect(() => {
    if (currStatus !== PossibleAdoptionStatus.Approved) {
      setValue("eventTitle", null);
      setValue("eventDescription", null);
      setValue("eventDate", null);
      setValue("eventLocation", null);
    } else {
      setValue(
        "eventTitle",
        `${t("adoptions.edit.event.title")}${adoption.animalName}`
      );
      setValue("eventLocation", t("adoptions.edit.event.location"));
    }
  }, [currStatus, setValue, t, adoption.animalName]);

  return (
    <Portal>
      <Dialog.Root
        open={isOpen}
        onOpenChange={() => {
          onClose();
          reset();
        }}
        placement="top"
        size={"lg"}
        motionPreset="slide-in-bottom"
      >
        <Dialog.Backdrop />
        <Dialog.Positioner>
          <Dialog.Content>
            <Dialog.Header>
              <Dialog.Title>{t("adoptions.edit.dialogTitle")}</Dialog.Title>
              <Dialog.CloseTrigger asChild>
                <CloseButton size="sm" />
              </Dialog.CloseTrigger>
            </Dialog.Header>
            <form onSubmit={handleSubmit(onSubmit)}>
              <Dialog.Body>
                <Box display="flex" flexDirection="column" gap={4} p={4}>
                  <Field.Root>
                    <Field.Label>{t("animals.edit.status")}</Field.Label>
                    <NativeSelect.Root>
                      <NativeSelect.Field
                        {...register("status", { valueAsNumber: true })}
                      >
                        {Object.entries(PossibleAdoptionStatus).map(
                          ([key, value]) => (
                            <option key={key} value={value}>
                              {t(`adoptions.status.${key.toLowerCase()}`)}
                            </option>
                          )
                        )}
                      </NativeSelect.Field>
                      <NativeSelect.Indicator />
                    </NativeSelect.Root>
                  </Field.Root>
                  <Field.Root>
                    <Field.Label>
                      {t("adoptions.fields.note")}
                      <Field.RequiredIndicator
                        fallback={
                          <Badge size="xs" variant="surface">
                            {t("optional")}
                          </Badge>
                        }
                      />
                    </Field.Label>
                    <Textarea
                      variant={"outline"}
                      placeholder={t("adoptions.fields.notePlaceholder")}
                      {...register("note")}
                    />
                    {errors.note && (
                      <Text color={"red"}>
                        {getFormErrorMessage(errors.note.message)}
                      </Text>
                    )}
                  </Field.Root>
                  {currStatus === PossibleAdoptionStatus.Approved && (
                    <>
                      <Separator marginY={3} />
                      <Heading size={"lg"}>
                        {t("adoptions.edit.event.section")}
                      </Heading>
                      <Field.Root>
                        <Field.Label>{t("events.fields.title")}</Field.Label>
                        <Input
                          variant={"outline"}
                          {...register("eventTitle")}
                        />
                        {errors.eventTitle && (
                          <Text color={"red"}>
                            {getFormErrorMessage(errors.eventTitle.message)}
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
                          {...register("eventDescription", {
                            setValueAs: (v) =>
                              v === "" || v === null ? null : v.toString(),
                          })}
                        />
                        {errors.eventDescription && (
                          <Text color={"red"}>
                            {getFormErrorMessage(
                              errors.eventDescription?.message
                            )}
                          </Text>
                        )}
                      </Field.Root>
                      <Field.Root>
                        <Field.Label>
                          {t("events.fields.date")}
                          <Field.RequiredIndicator />
                        </Field.Label>
                        <Input
                          type="datetime-local"
                          {...register("eventDate", {
                            valueAsDate: true,
                          })}
                        />
                        {errors.eventDate && (
                          <Text color={"red"}>
                            {getFormErrorMessage(errors.eventDate.message)}
                          </Text>
                        )}
                      </Field.Root>
                      <Field.Root>
                        <Field.Label>{t("events.fields.location")}</Field.Label>
                        <Input
                          variant={"outline"}
                          {...register("eventLocation")}
                        />
                        {errors.eventLocation && (
                          <Text color={"red"}>
                            {getFormErrorMessage(errors.eventLocation.message)}
                          </Text>
                        )}
                      </Field.Root>
                    </>
                  )}
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
      </Dialog.Root>
    </Portal>
  );
};
