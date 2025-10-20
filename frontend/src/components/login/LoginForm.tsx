import { Button, Field, Input, Link, Stack, VStack } from "@chakra-ui/react";
import { PasswordInput } from "../ui/password-input";
import { useTranslation } from "react-i18next";

export const LoginForm = () => {
  const { t } = useTranslation();

  return (
    <Stack gap="4" align="flex-start" maxW="sm">
      <Field.Root>
        <Field.Label>{t("login.email")}</Field.Label>
        <Input />
        <Field.ErrorText>ab</Field.ErrorText>
      </Field.Root>

      <Field.Root>
        <Field.Label>{t("login.password")}</Field.Label>
        <PasswordInput />
        <Field.ErrorText>abc</Field.ErrorText>
      </Field.Root>
      <Link fontSize={"sm"} color={"green.400"} href="/forgot-password">
        {t("login.forgotPassword")}
      </Link>
      <VStack width={"100%"} justifyContent={"center"}>
        <Button type="submit" background={"green.400"} size={"lg"}>
          {t("login.login")}
        </Button>
      </VStack>
    </Stack>
  );
};
