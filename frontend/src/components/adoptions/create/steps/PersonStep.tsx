import {
  Box,
  Button,
  Field,
  Flex,
  Heading,
  Input,
  Text,
} from "@chakra-ui/react";
import { useFormContext } from "react-hook-form";
import { useTranslation } from "react-i18next";
import type { AddAdoptionSchema } from "../AddAdoptionContext";
import { getFormErrorMessage } from "../../../../utils";

interface Props {
  onNext: () => void;
  onBack: () => void;
}

export const PersonStep = ({ onNext, onBack }: Props) => {
  const { t } = useTranslation();
  const {
    register,
    trigger,
    formState: { errors },
  } = useFormContext<AddAdoptionSchema>();

  const handleNext = async () => {
    const isValid = await trigger([
      "personName",
      "personSurname",
      "personPhoneNumber",
      "personEmail",
      "personPesel",
      "personDocumentId",
      "personPostalCode",
      "personCity",
      "personStreet",
    ]);

    if (isValid) {
      onNext();
    }
  };

  return (
    <Box display="flex" flexDirection="column" gap={4} p={4}>
      <Heading>{t("adoptions.create.person")}</Heading>
      <Flex gap={5}>
        <Field.Root maxW={"50%"}>
          <Field.Label>
            {t("adoptions.fields.person.name")}
            <Field.RequiredIndicator />
          </Field.Label>
          <Input
            placeholder={t("adoptions.fields.person.namePlaceholder")}
            {...register("personName")}
          />
          {errors.personName && (
            <Text color={"red"}>
              {getFormErrorMessage(errors.personName?.message)}
            </Text>
          )}
        </Field.Root>
        <Field.Root maxW={"50%"}>
          <Field.Label>
            {t("adoptions.fields.person.surname")}
            <Field.RequiredIndicator />
          </Field.Label>
          <Input
            placeholder={t("adoptions.fields.person.surnamePlaceholder")}
            {...register("personSurname")}
          />
          {errors.personSurname && (
            <Text color={"red"}>
              {getFormErrorMessage(errors.personSurname?.message)}
            </Text>
          )}
        </Field.Root>
      </Flex>
      <Field.Root>
        <Field.Label>
          {t("adoptions.fields.person.phone")}
          <Field.RequiredIndicator />
        </Field.Label>
        <Input
          placeholder={t("adoptions.fields.person.phonePlaceholder")}
          {...register("personPhoneNumber")}
        />
        {errors.personPhoneNumber && (
          <Text color={"red"}>
            {getFormErrorMessage(errors.personPhoneNumber?.message)}
          </Text>
        )}
      </Field.Root>
      <Field.Root>
        <Field.Label>
          {t("adoptions.fields.person.email")}
          <Field.RequiredIndicator />
        </Field.Label>
        <Input
          placeholder={t("adoptions.fields.person.emailPlaceholder")}
          {...register("personEmail")}
        />
        {errors.personEmail && (
          <Text color={"red"}>
            {getFormErrorMessage(errors.personEmail?.message)}
          </Text>
        )}
      </Field.Root>
      <Field.Root>
        <Field.Label>
          {t("adoptions.fields.person.pesel")}
          <Field.RequiredIndicator />
        </Field.Label>
        <Input
          placeholder={t("adoptions.fields.person.peselPlaceholder")}
          {...register("personPesel")}
        />
        {errors.personPesel && (
          <Text color={"red"}>
            {getFormErrorMessage(errors.personPesel?.message)}
          </Text>
        )}
      </Field.Root>
      <Field.Root>
        <Field.Label>
          {t("adoptions.fields.person.document")}
          <Field.RequiredIndicator />
        </Field.Label>
        <Input
          placeholder={t("adoptions.fields.person.documentPlaceholder")}
          {...register("personDocumentId")}
        />
        {errors.personDocumentId && (
          <Text color={"red"}>
            {getFormErrorMessage(errors.personDocumentId?.message)}
          </Text>
        )}
      </Field.Root>
      <Flex gap={5}>
        <Field.Root w="30%">
          <Field.Label>
            {t("adoptions.fields.person.postalCode")}
            <Field.RequiredIndicator />
          </Field.Label>
          <Input
            placeholder={t("adoptions.fields.person.postalCodePlaceholder")}
            {...register("personPostalCode")}
          />
          {errors.personPostalCode && (
            <Text color={"red"}>
              {getFormErrorMessage(errors.personPostalCode?.message)}
            </Text>
          )}
        </Field.Root>
        <Field.Root>
          <Field.Label>
            {t("adoptions.fields.person.city")}
            <Field.RequiredIndicator />
          </Field.Label>
          <Input
            placeholder={t("adoptions.fields.person.cityPlaceholder")}
            {...register("personCity")}
          />
          {errors.personCity && (
            <Text color={"red"}>
              {getFormErrorMessage(errors.personCity?.message)}
            </Text>
          )}
        </Field.Root>
      </Flex>
      <Field.Root>
        <Field.Label>
          {t("adoptions.fields.person.street")}
          <Field.RequiredIndicator />
        </Field.Label>
        <Input
          placeholder={t("adoptions.fields.person.streetPlaceholder")}
          {...register("personStreet")}
        />
        {errors.personStreet && (
          <Text color={"red"}>
            {getFormErrorMessage(errors.personStreet?.message)}
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
