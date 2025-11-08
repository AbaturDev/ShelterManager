import {
  Badge,
  Box,
  Button,
  Field,
  Flex,
  Heading,
  NativeSelect,
  Text,
  Textarea,
} from "@chakra-ui/react";
import { useFormContext } from "react-hook-form";
import { useTranslation } from "react-i18next";
import type { AddAdoptionSchema } from "../AddAdoptionContext";
import { getFormErrorMessage } from "../../../../utils";
import { useAnimalsListQuery } from "../../../../hooks/useAnimalsListQuery";

interface Props {
  onNext: () => void;
}

export const GeneralStep = ({ onNext }: Props) => {
  const { t } = useTranslation();
  const {
    register,
    formState: { errors },
    setValue,
    watch,
    trigger,
  } = useFormContext<AddAdoptionSchema>();

  const { data: animalsData } = useAnimalsListQuery({
    page: 1,
    pageSize: 10000000,
  });

  const handleNext = async () => {
    const isValid = await trigger(["animalId", "animalName", "note"]);
    if (isValid) {
      onNext();
    }
  };

  return (
    <Box display="flex" flexDirection="column" gap={4} p={4}>
      <Heading>{t("adoptions.create.general")}</Heading>
      <Field.Root>
        <Field.Label>
          {t("adoptions.fields.animal")}
          <Field.RequiredIndicator />
        </Field.Label>
        <NativeSelect.Root>
          <NativeSelect.Field
            defaultValue={watch("animalId")}
            onChange={(e) => {
              setValue("animalId", e.target.value);
              setValue(
                "animalName",
                animalsData?.items.find((a) => a.id === e.target.value)?.name ||
                  ""
              );
            }}
          >
            <option value="">{t("adoptions.fields.selectAnimal")}</option>
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
          placeholder={t("adoptions.fields.notePlaceholder")}
          {...register("note")}
        />
        {errors.note && (
          <Text color={"red"}>{getFormErrorMessage(errors.note?.message)}</Text>
        )}
      </Field.Root>
      <Flex w="100%" justifyContent={"end"}>
        <Button background="green.400" mt={4} onClick={handleNext}>
          {t("next")}
        </Button>
      </Flex>
    </Box>
  );
};
