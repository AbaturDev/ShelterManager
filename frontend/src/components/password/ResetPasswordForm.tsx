import {
  Box,
  Flex,
  Field,
  Heading,
  Stack,
  Button,
  Text,
  VStack,
  Link,
} from "@chakra-ui/react";
import { PasswordInput } from "../ui/password-input";
import { useTranslation } from "react-i18next";
import z from "zod";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import {
  getFormErrorMessage,
  setFormErrorMessage,
} from "../../utils/form-error-handlers";
import { AccountService } from "../../api/services/account-service";
import { Navigate } from "react-router-dom";
import { useState } from "react";

const passwordComplexityRegex =
  /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\w\s]).{8,}$/;

const schema = z
  .object({
    password: z
      .string()
      .min(8, setFormErrorMessage("resetPassword.schema.min"))
      .regex(
        passwordComplexityRegex,
        setFormErrorMessage("resetPassword.schema.regex")
      ),

    confirmPassword: z.string(),
  })
  .refine((data) => data.password === data.confirmPassword, {
    message: setFormErrorMessage("resetPassword.schema.identical"),
    path: ["confirmNewPassword"],
  });

type FormData = z.infer<typeof schema>;

interface ResetPasswordFormProps {
  token: string | null;
  email: string | null;
}

export const ResetPasswordForm = ({ token, email }: ResetPasswordFormProps) => {
  if (token === null || email === null) {
    return <Navigate to="/login" />;
  }

  const { t } = useTranslation();

  const [isError, setIsError] = useState(false);
  const [isSent, setIsSent] = useState(false);

  const {
    register,
    handleSubmit,
    formState: { isSubmitting, errors },
    reset,
  } = useForm<FormData>({ resolver: zodResolver(schema) });

  const onSubmit = async (data: FormData) => {
    try {
      await AccountService.resetPassword({
        email: email,
        newPassword: data.password,
        token: token,
      });
      setIsError(false);
    } catch (err) {
      setIsError(true);
    } finally {
      setIsSent(true);
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
      w="lg"
    >
      <Flex w="100%" justifyContent={"center"}>
        <VStack gap={3}>
          <Heading fontSize={"2xl"}>{t("resetPassword.title")}</Heading>
          {!isSent && (
            <Text textAlign={"center"}>{t("resetPassword.instruction")}</Text>
          )}
        </VStack>
      </Flex>
      {!isSent ? (
        <form onSubmit={handleSubmit(onSubmit)}>
          <Stack padding={5} align="flex-start">
            <Field.Root>
              <Field.Label>{t("resetPassword.password")}</Field.Label>
              <PasswordInput {...register("password")} />
              {errors.password && (
                <Text color="red">
                  {getFormErrorMessage(errors.password.message)}
                </Text>
              )}
            </Field.Root>
            <Field.Root>
              <Field.Label>{t("resetPassword.confirmPassword")}</Field.Label>
              <PasswordInput {...register("confirmPassword")} />
              {errors.confirmPassword && (
                <Text color="red">
                  {getFormErrorMessage(errors.confirmPassword.message)}
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
                {t("resetPassword.button")}
              </Button>
              <Link fontSize="sm" color="green.400" href="/login">
                {t("resetPassword.backToLogin")}
              </Link>
            </VStack>
          </Flex>
        </form>
      ) : (
        <Flex justifyContent="center" mt={4}>
          <VStack>
            <Text color={isError ? "red.600" : "green.600"} textAlign="center">
              {isError ? t("resetPassword.error") : t("resetPassword.success")}
            </Text>
            <Link fontSize="sm" color="green.400" href="/login">
              {t("resetPassword.backToLogin")}
            </Link>
          </VStack>
        </Flex>
      )}
    </Box>
  );
};
