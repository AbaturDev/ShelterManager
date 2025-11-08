import { Box, Button, Card, Text, Stack } from "@chakra-ui/react";
import { useFormContext } from "react-hook-form";
import { useTranslation } from "react-i18next";
import {
  useAddAdoptionContext,
  type AddAdoptionSchema,
} from "../AddAdoptionContext";

interface Props {
  onBack: () => void;
  onClose: () => void;
}

export const SummaryStep = ({ onBack, onClose }: Props) => {
  const { t } = useTranslation();
  const {
    watch,
    handleSubmit,
    formState: { isValid },
  } = useFormContext<AddAdoptionSchema>();
  const { addAdoptionMutation } = useAddAdoptionContext();

  const formData = watch();

  const onSubmit = handleSubmit(async (data) => {
    await addAdoptionMutation.mutateAsync({
      note: data.note ?? undefined,
      animalId: data.animalId,
      person: {
        name: data.personName,
        surname: data.personSurname,
        phoneNumber: data.personPhoneNumber,
        email: data.personEmail,
        pesel: data.personPesel,
        city: data.personCity,
        postalCode: data.personPostalCode,
        street: data.personStreet,
        documentId: data.personDocumentId,
      },
    });
  });

  return (
    <>
      <Card.Root>
        <Card.Header>
          <Text fontSize="xl" fontWeight="bold">
            {t("adoptions.create.summary")}
          </Text>
        </Card.Header>
        <Card.Body>
          <Stack gap={4}>
            <Box>
              <Text fontWeight="bold" color="gray.600" fontSize="sm">
                {t("adoptions.fields.animal")}
              </Text>
              <Text fontSize="lg">{formData.animalName || "-"}</Text>
            </Box>

            <Box>
              <Text fontWeight="bold" color="gray.600" fontSize="sm">
                {t("adoptions.fields.note")}
              </Text>
              <Text fontSize="lg" whiteSpace="pre-wrap">
                {formData.note || "-"}
              </Text>
            </Box>

            <Box>
              <Text fontWeight="bold" color="gray.600" fontSize="sm">
                {t("adoptions.create.person")}
              </Text>
              <Stack pl={4} mt={1}>
                <Text>
                  <strong>{t("adoptions.fields.person.name")}:</strong>{" "}
                  {formData.personName || "-"}
                </Text>
                <Text>
                  <strong>{t("adoptions.fields.person.surname")}:</strong>{" "}
                  {formData.personSurname || "-"}
                </Text>
                <Text>
                  <strong>{t("adoptions.fields.person.phone")}:</strong>{" "}
                  {formData.personPhoneNumber || "-"}
                </Text>
                <Text>
                  <strong>{t("adoptions.fields.person.email")}:</strong>{" "}
                  {formData.personEmail || "-"}
                </Text>
                <Text>
                  <strong>{t("adoptions.fields.person.pesel")}:</strong>{" "}
                  {formData.personPesel || "-"}
                </Text>
                <Text>
                  <strong>{t("adoptions.fields.person.document")}:</strong>{" "}
                  {formData.personDocumentId || "-"}
                </Text>
                <Text>
                  <strong>{t("adoptions.fields.person.postalCode")}:</strong>{" "}
                  {formData.personPostalCode || "-"}
                </Text>
                <Text>
                  <strong>{t("adoptions.fields.person.city")}:</strong>{" "}
                  {formData.personCity || "-"}
                </Text>
                <Text>
                  <strong>{t("adoptions.fields.person.street")}:</strong>{" "}
                  {formData.personStreet || "-"}
                </Text>
              </Stack>
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
            {t("adoptions.create.validationError")}
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
          loading={addAdoptionMutation.isPending}
          disabled={!isValid}
        >
          {t("save")}
        </Button>
      </Box>
    </>
  );
};
