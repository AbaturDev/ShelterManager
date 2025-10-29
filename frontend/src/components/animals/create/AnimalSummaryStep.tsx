import { Box, Button, Card, Text, Stack } from "@chakra-ui/react";
import { useFormContext } from "react-hook-form";
import { useTranslation } from "react-i18next";
import type { AddAnimalSchema } from "./AddAnimal.Context";
import { useAddAnimalContext } from "./AddAnimal.Context";
import type { SexType } from "../../../models/animal";

interface Props {
  onBack: () => void;
  onClose: () => void;
}

export const AnimalSummaryStep = ({ onBack, onClose }: Props) => {
  const { t } = useTranslation();
  const { watch, handleSubmit } = useFormContext<AddAnimalSchema>();
  const { addAnimalMutation } = useAddAnimalContext();

  const formData = watch();

  const formatDate = (date: Date | null | undefined) => {
    if (!date || !(date instanceof Date) || isNaN(date.getTime())) {
      return "-";
    }
    return new Intl.DateTimeFormat("pl-PL", {
      year: "numeric",
      month: "long",
      day: "numeric",
    }).format(date);
  };

  const onSubmit = handleSubmit(async (data) => {
    const age = data.age ?? undefined;
    const description = data.description ?? undefined;

    await addAnimalMutation.mutateAsync({
      name: data.name,
      admissionDate: data.admissionDate.toISOString(),
      age: age,
      sex: data.sex as SexType,
      breedId: data.breedId,
      description: description,
    });
  });

  const isValid =
    !!formData.name &&
    !!formData.admissionDate &&
    !!formData.breedId &&
    !!formData.species &&
    !!formData.breed &&
    !isNaN(formData.admissionDate.getTime());

  return (
    <>
      <Card.Root>
        <Card.Header>
          <Text fontSize="xl" fontWeight="bold">
            {t("animals.create.summary.title")}
          </Text>
        </Card.Header>
        <Card.Body>
          <Stack gap={4}>
            <Box>
              <Text fontWeight="bold" color="gray.600" fontSize="sm">
                {t("animals.create.fields.name")}
              </Text>
              <Text fontSize="lg">{formData.name || "-"}</Text>
            </Box>

            <Box>
              <Text fontWeight="bold" color="gray.600" fontSize="sm">
                {t("animals.create.fields.admissionDate")}
              </Text>
              <Text fontSize="lg">{formatDate(formData.admissionDate)}</Text>
            </Box>

            <Box>
              <Text fontWeight="bold" color="gray.600" fontSize="sm">
                {t("animals.create.fields.sex")}
              </Text>
              <Text fontSize="lg">
                {formData.sex !== undefined
                  ? t(`animals.sex.${formData.sex}`)
                  : "-"}
              </Text>
            </Box>

            <Box>
              <Text fontWeight="bold" color="gray.600" fontSize="sm">
                {t("animals.create.fields.age")}
              </Text>
              <Text fontSize="lg">
                {formData.age
                  ? `${formData.age} ${t("animals.list.years")}`
                  : t("animals.unknown")}
              </Text>
            </Box>

            <Box>
              <Text fontWeight="bold" color="gray.600" fontSize="sm">
                {t("animals.create.fields.species")}
              </Text>
              <Text fontSize="lg">{formData.species || "-"}</Text>
            </Box>

            <Box>
              <Text fontWeight="bold" color="gray.600" fontSize="sm">
                {t("animals.create.fields.breed")}
              </Text>
              <Text fontSize="lg">{formData.breed || "-"}</Text>
            </Box>

            <Box>
              <Text fontWeight="bold" color="gray.600" fontSize="sm">
                {t("animals.create.fields.description")}
              </Text>
              <Text fontSize="lg" whiteSpace="pre-wrap">
                {formData.description || "-"}
              </Text>
            </Box>
          </Stack>
        </Card.Body>
      </Card.Root>

      {!isValid && (
        <Box
          mt={4}
          p={3}
          bg="red.50"
          borderLeft="4px solid"
          borderColor="red.400"
          borderRadius="md"
        >
          <Text color="orange.700" fontSize="sm" fontWeight="medium">
            {t("animals.create.summary.validationError")}
          </Text>
        </Box>
      )}

      <Box display="flex" justifyContent="space-between" mt={4}>
        <Button variant="outline" onClick={onBack}>
          {t("prev")}
        </Button>
        <Button
          background="green.400"
          onClick={() => {
            onSubmit();
            onClose();
          }}
          loading={addAnimalMutation.isPending}
          disabled={!isValid}
        >
          {t("save")}
        </Button>
      </Box>
    </>
  );
};
