import {
  Badge,
  Box,
  Button,
  Field,
  Flex,
  Input,
  NativeSelect,
  Select,
  Text,
  Textarea,
} from "@chakra-ui/react";
import { useFormContext, Controller } from "react-hook-form";
import { useTranslation } from "react-i18next";
import { getFormErrorMessage } from "../../../utils/form-error-handlers";
import type { AddAnimalSchema } from "./AddAnimal.Context";

interface Props {
  onNext: () => void;
}

export const AnimalDetailsStep = ({ onNext }: Props) => {
  const { t } = useTranslation();
  const {
    control,
    register,
    formState: { errors },
  } = useFormContext<AddAnimalSchema>();

  return (
    <Box display="flex" flexDirection="column" gap={4} p={4}>
      <Field.Root>
        <Field.Label>
          {t("animals.create.fields.name")}
          <Field.RequiredIndicator />
        </Field.Label>
        <Input
          placeholder={t("animals.create.fields.name")}
          {...register("name")}
        />
        {errors.name && (
          <Text color={"red"}>{getFormErrorMessage(errors.name?.message)}</Text>
        )}
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
          min={0}
          max={100}
          placeholder={t("animals.create.fields.age")}
          {...register("age")}
        />
      </Field.Root>
      <Field.Root>
        <Field.Label>{t("animals.create.fields.sex")}</Field.Label>
        <NativeSelect.Root>
          <NativeSelect.Field {...register("sex")}>
            <option value={0}>{t("animals.sex.0")}</option>
            <option value={1}>{t("animals.sex.1")}</option>
          </NativeSelect.Field>
          <NativeSelect.Indicator />
        </NativeSelect.Root>
      </Field.Root>
      <Field.Root>
        <Field.Label>
          {t("animals.create.fields.admissionDate")} <Field.RequiredIndicator />
        </Field.Label>
        <Input
          type="date"
          {...register("admissionDate", { valueAsDate: true })}
        />
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
          placeholder={t("animals.create.fields.description")}
          {...register("description")}
        />
      </Field.Root>
      <Flex w="100%" justifyContent={"end"}>
        <Button background="green.400" mt={4} onClick={onNext}>
          {t("next")}
        </Button>
      </Flex>
    </Box>
  );
};
