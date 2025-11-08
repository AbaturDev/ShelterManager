import { Box, Button, Field, NativeSelect, Text } from "@chakra-ui/react";
import { useFormContext, Controller } from "react-hook-form";
import { useTranslation } from "react-i18next";
import { useSpeciesListQuery } from "../../../hooks/useSpeciesListQuery";
import { useBreedsListQuery } from "../../../hooks/useBreedsListQuery";
import type { AddAnimalSchema } from "./AddAnimal.Context";
import { getFormErrorMessage } from "../../../utils/form-error-handlers";
import { useState } from "react";

interface Props {
  onNext: () => void;
  onBack: () => void;
}

export const AnimalSpeciesStep = ({ onNext, onBack }: Props) => {
  const { t } = useTranslation();
  const {
    control,
    setValue,
    formState: { errors },
    watch,
    trigger,
  } = useFormContext<AddAnimalSchema>();

  const speciesId = watch("speciesId");
  const [selectedSpeciesId, setSelectedSpeciesId] = useState<string>(
    speciesId || ""
  );

  const { data: speciesData } = useSpeciesListQuery(1, 10000000);
  const { data: breedsData } = useBreedsListQuery(
    selectedSpeciesId,
    1,
    10000000
  );

  const handleSpeciesChange = (speciesId: string, speciesName: string) => {
    setSelectedSpeciesId(speciesId);
    setValue("speciesId", speciesId);
    setValue("species", speciesName);
    setValue("breed", "");
    setValue("breedId", "");
  };

  const handleNext = async () => {
    const isValid = await trigger(["speciesId", "species", "breed", "breedId"]);
    if (isValid) {
      onNext();
    }
  };

  return (
    <Box display="flex" flexDirection="column" gap={4} p={4}>
      <Field.Root>
        <Field.Label>
          {t("animals.create.fields.species")}
          <Field.RequiredIndicator />
        </Field.Label>
        <NativeSelect.Root>
          <NativeSelect.Field
            value={selectedSpeciesId}
            onChange={(e) => {
              const speciesId = e.target.value;
              const speciesName =
                speciesData?.items.find((s) => s.id === speciesId)?.name || "";
              handleSpeciesChange(speciesId, speciesName);
            }}
          >
            <option value="">{t("animals.create.fields.selectSpecies")}</option>
            {speciesData?.items.map((s) => (
              <option key={s.id} value={s.id}>
                {s.name}
              </option>
            ))}
          </NativeSelect.Field>
          <NativeSelect.Indicator />
        </NativeSelect.Root>
        {errors.species && (
          <Text color="red">
            {getFormErrorMessage(errors.species?.message)}
          </Text>
        )}
      </Field.Root>

      <Field.Root>
        <Field.Label>
          {t("animals.create.fields.breed")}
          <Field.RequiredIndicator />
        </Field.Label>
        <Controller
          control={control}
          name="breedId"
          render={({ field }) => (
            <NativeSelect.Root>
              <NativeSelect.Field
                {...field}
                onChange={(e) => {
                  const breedId = e.target.value;
                  const breedName =
                    breedsData?.items.find((b) => b.id === breedId)?.name || "";
                  field.onChange(breedId);
                  setValue("breed", breedName);
                }}
              >
                <option value="">
                  {!selectedSpeciesId
                    ? t("animals.create.fields.selectSpeciesFirst")
                    : t("animals.create.fields.selectBreed")}
                </option>
                {breedsData?.items.map((b) => (
                  <option key={b.id} value={b.id}>
                    {b.name}
                  </option>
                ))}
              </NativeSelect.Field>
              <NativeSelect.Indicator />
            </NativeSelect.Root>
          )}
        />
        {errors.breedId && (
          <Text color="red">
            {getFormErrorMessage(errors.breedId?.message)}
          </Text>
        )}
      </Field.Root>

      <Box display="flex" justifyContent="space-between" mt={4}>
        <Button variant="outline" onClick={onBack}>
          {t("prev")}
        </Button>
        <Button background="green.400" onClick={handleNext}>
          {t("next")}
        </Button>
      </Box>
    </Box>
  );
};
