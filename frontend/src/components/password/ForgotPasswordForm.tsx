import { zodResolver } from "@hookform/resolvers/zod";
import { useForm } from "react-hook-form";
import { useTranslation } from "react-i18next";
import z from "zod";
import { AccountService } from "../../api/services/account-service";
import {
  Box,
  Button,
  Field,
  Flex,
  Heading,
  Input,
  Link,
  Stack,
  Text,
  VStack,
} from "@chakra-ui/react";
import {
  getFormErrorMessage,
  setFormErrorMessage,
} from "../../utils/form-error-handlers";
import { useState } from "react";

const schema = z.object({
  email: z.string().email(setFormErrorMessage("forgotPassword.schema.email")),
});

type FormData = z.infer<typeof schema>;

export const ForgotPasswordForm = () => {
  const [isEmailSent, setIsEmailSent] = useState(false);
  const { t, i18n } = useTranslation();

  const currentLang = i18n.language;

  const {
    register,
    handleSubmit,
    formState: { isSubmitting, errors },
    reset,
  } = useForm<FormData>({ resolver: zodResolver(schema) });

  const onSubmit = async (data: FormData) => {
    try {
      await AccountService.forgotPassword({ email: data.email }, currentLang);
    } finally {
      setIsEmailSent(true);
      reset();
    }
  };

  return (
    <Box
      marginTop={5}
      p={5}
      borderWidth="1px"
      borderRadius="md"
      boxShadow="md"
      bg="white"
      w="md"
    >
      <Flex w="100%" justifyContent={"center"}>
        <VStack gap={3}>
          <Heading fontSize={"2xl"}>{t("forgotPassword.header")}</Heading>
          {!isEmailSent && (
            <Text textAlign={"center"}>{t("forgotPassword.instruction")}</Text>
          )}
        </VStack>
      </Flex>
      {!isEmailSent ? (
        <form onSubmit={handleSubmit(onSubmit)}>
          <Stack padding={3} align="flex-start">
            <Field.Root>
              <Field.Label>{t("forgotPassword.emailInput")}</Field.Label>
              <Input {...register("email")} />
              {errors.email && (
                <Text color="red">
                  {getFormErrorMessage(errors.email.message)}
                </Text>
              )}
            </Field.Root>
          </Stack>
          <Flex w="100%" justifyContent={"center"} padding={3}>
            <VStack gap={3}>
              <Button
                type="submit"
                background={"green.400"}
                size="lg"
                disabled={isSubmitting}
              >
                {t("forgotPassword.button")}
              </Button>
              <Link fontSize={"sm"} color={"green.400"} href="/login">
                {t("forgotPassword.backToLogin")}
              </Link>
            </VStack>
          </Flex>
        </form>
      ) : (
        <Flex w="100%" justifyContent="center" padding={5}>
          <VStack gap={3}>
            <Text textAlign="center" color="green.600" fontWeight="medium">
              {t("forgotPassword.emailSentMessage")}
            </Text>
            <Link fontSize="sm" color="green.400" href="/login">
              {t("forgotPassword.backToLogin")}
            </Link>
          </VStack>
        </Flex>
      )}
    </Box>
  );
};
