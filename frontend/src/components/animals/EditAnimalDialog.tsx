import {
  Badge,
  Box,
  Button,
  Center,
  CloseButton,
  Dialog,
  Field,
  HStack,
  Input,
  NativeSelect,
  Portal,
  Spinner,
  Text,
  Textarea,
} from "@chakra-ui/react";
import { useTranslation } from "react-i18next";
import z from "zod";
import {
  formatDate,
  getFormErrorMessage,
  setFormErrorMessage,
} from "../../utils";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { AnimalService } from "../../api/services/animals-service";
import { toaster } from "../ui/toaster";
import type { Animal, AnimalStatusType } from "../../models/animal";
import { useAnimalByIdQuery } from "../../hooks/useAnimalByIdQuery";
import { useEffect } from "react";

const schema = z.object({
  name: z
    .string()
    .min(3, setFormErrorMessage("animals.create.errors.nameMin"))
    .max(30, setFormErrorMessage("animals.create.errors.nameMax")),
  admissionDate: z
    .date(setFormErrorMessage("animals.create.errors.date"))
    .max(new Date(), setFormErrorMessage("animals.create.errors.dateFuture")),
  age: z
    .number()
    .min(0, setFormErrorMessage("animals.create.errors.ageMin"))
    .max(100, setFormErrorMessage("animals.create.errors.ageMax"))
    .nullable(),
  description: z
    .string()
    .max(350, setFormErrorMessage("animals.create.errors.description"))
    .nullable(),
  status: z.number(),
});

type FormData = z.infer<typeof schema>;

interface EditAnimalDialogProps {
  id: string;
  isOpen: boolean;
  onClose: () => void;
  onSuccess: () => void;
}

export const EditAnimalDialog = ({
  id,
  isOpen,
  onClose,
  onSuccess,
}: EditAnimalDialogProps) => {
  const queryClient = useQueryClient();
  const { t } = useTranslation();

  const { data, error, isLoading, refetch } = useAnimalByIdQuery(id, {
    enabled: isOpen,
  });

  if (error) {
    toaster.create({
      type: "error",
      title: t("error"),
      description: t("animals.fetchError"),
      closable: true,
    });
    onClose();
  }

  const {
    register,
    formState: { errors },
    handleSubmit,
    reset,
  } = useForm<FormData>({
    resolver: zodResolver(schema),
    mode: "onChange",
  });

  const mutation = useMutation({
    mutationFn: (data: FormData) =>
      AnimalService.editAnimal(id, {
        name: data.name,
        admissionDate: data.admissionDate.toISOString(),
        age: data.age ?? undefined,
        status: data.status as AnimalStatusType,
        description: data.description ?? undefined,
      }),
    onSuccess: () => {
      queryClient.invalidateQueries({
        queryKey: [`animals`, id],
      });
      queryClient.invalidateQueries({
        queryKey: [`animals`],
      });
      toaster.create({
        type: "success",
        title: t("success"),
        description: t("animals.edit.toast.success"),
        closable: true,
      });
      onSuccess();
      reset();
    },
    onError: () => {
      toaster.create({
        type: "error",
        title: t("error"),
        description: t("animals.edit.toast.error"),
        closable: true,
      });
      onClose();
      reset();
    },
  });

  useEffect(() => {
    if (isOpen) {
      reset();
      refetch();
    }
  }, [isOpen, refetch]);

  const onSubmit = async (data: FormData) => await mutation.mutateAsync(data);

  const animal = data as Animal;

  return (
    <Portal>
      <form onSubmit={handleSubmit(onSubmit)}>
        <Dialog.Root
          open={isOpen}
          onOpenChange={onClose}
          motionPreset="slide-in-bottom"
          size={"xl"}
        >
          <Dialog.Backdrop />
          <Dialog.Positioner>
            <Dialog.Content>
              <Dialog.Header
                justifyContent={"space-between"}
                alignContent={"center"}
              >
                <Dialog.Title>{t("animals.edit.title")}</Dialog.Title>
                <Dialog.CloseTrigger asChild>
                  <CloseButton size="sm" />
                </Dialog.CloseTrigger>
              </Dialog.Header>
              <Dialog.Body>
                {isLoading ? (
                  <Center height="100%" bg="gray.100">
                    <Spinner size="xl" color="blue.500" />
                  </Center>
                ) : (
                  <Box display="flex" flexDirection="column" gap={4} p={4}>
                    <Field.Root>
                      <Field.Label>
                        {t("animals.create.fields.name")}
                        <Field.RequiredIndicator />
                      </Field.Label>
                      <Input
                        defaultValue={animal?.name}
                        placeholder={t("animals.create.fields.name")}
                        {...register("name")}
                      />
                      {errors.name && (
                        <Text color={"red"}>
                          {getFormErrorMessage(errors.name?.message)}
                        </Text>
                      )}
                    </Field.Root>
                    <Field.Root>
                      <Field.Label>{t("animals.edit.status")}</Field.Label>
                      <NativeSelect.Root>
                        <NativeSelect.Field
                          defaultValue={animal?.status}
                          {...register("status", { valueAsNumber: true })}
                        >
                          <option value={0}>
                            {t("animals.status.inShelter")}
                          </option>
                          <option value={1}>
                            {t("animals.status.adopted")}
                          </option>
                          <option value={2}>{t("animals.status.died")}</option>
                        </NativeSelect.Field>
                        <NativeSelect.Indicator />
                      </NativeSelect.Root>
                    </Field.Root>
                    <Field.Root>
                      <Field.Label>
                        {t("animals.create.fields.age")}
                        <Field.RequiredIndicator
                          fallback={
                            <Badge size="xs" variant="surface">
                              {t("optional")}
                            </Badge>
                          }
                        />
                      </Field.Label>
                      <Input
                        type="number"
                        defaultValue={animal?.age ?? ""}
                        min={0}
                        max={100}
                        placeholder={t("animals.edit.age")}
                        {...register("age", {
                          setValueAs: (v) =>
                            v === "" || v === null || isNaN(Number(v))
                              ? null
                              : Number(v),
                        })}
                      />

                      {errors.age && (
                        <Text color={"red"}>
                          {getFormErrorMessage(errors.age?.message)}
                        </Text>
                      )}
                    </Field.Root>
                    <Field.Root>
                      <Field.Label>
                        {t("animals.create.fields.admissionDate")}{" "}
                        <Field.RequiredIndicator />
                      </Field.Label>
                      <Input
                        type="date"
                        defaultValue={formatDate(animal?.admissionDate)}
                        {...register("admissionDate", {
                          valueAsDate: true,
                        })}
                      />
                      {errors.admissionDate && (
                        <Text color={"red"}>
                          {getFormErrorMessage(errors.admissionDate.message)}
                        </Text>
                      )}
                    </Field.Root>
                    <Field.Root>
                      <Field.Label>
                        {t("animals.create.fields.description")}
                        <Field.RequiredIndicator
                          fallback={
                            <Badge size="xs" variant="surface">
                              {t("optional")}
                            </Badge>
                          }
                        />
                      </Field.Label>
                      <Textarea
                        defaultValue={animal?.description}
                        placeholder={t("animals.create.fields.description")}
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
                  </Box>
                )}
              </Dialog.Body>
              <Dialog.Footer>
                <HStack justify="space-between" w="100%">
                  <Button onClick={onClose} variant={"outline"}>
                    {t("cancel")}
                  </Button>
                  <Button type="submit" background={"green.400"}>
                    {t("save")}
                  </Button>
                </HStack>
              </Dialog.Footer>
            </Dialog.Content>
          </Dialog.Positioner>
        </Dialog.Root>
      </form>
    </Portal>
  );
};
