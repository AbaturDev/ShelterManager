import {
  Box,
  Flex,
  Field,
  Heading,
  Stack,
  Button,
  Text,
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
import { useAuth } from "../../utils/AuthProvider";
import { AccountService } from "../../api/services/account-service";
import { toaster } from "../ui/toaster";

const passwordComplexityRegex =
  /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\w\s]).{8,}$/;

const schema = z
  .object({
    oldPassword: z
      .string()
      .min(8, setFormErrorMessage("changePassword.schema.min")),

    newPassword: z
      .string()
      .min(8, setFormErrorMessage("changePassword.schema.min"))
      .regex(
        passwordComplexityRegex,
        setFormErrorMessage("changePassword.schema.regex")
      ),

    confirmNewPassword: z.string(),
  })
  .refine((data) => data.newPassword === data.confirmNewPassword, {
    message: setFormErrorMessage("changePassword.schema.identical"),
    path: ["confirmNewPassword"],
  })
  .refine((data) => data.newPassword !== data.oldPassword, {
    message: setFormErrorMessage("changePassword.schema.notSameAsOld"),
    path: ["newPassword"],
  });

type FormData = z.infer<typeof schema>;

export const ChangePasswordForm = () => {
  const { t } = useTranslation();
  const auth = useAuth();

  const {
    register,
    handleSubmit,
    formState: { isSubmitting, errors },
    reset,
  } = useForm<FormData>({ resolver: zodResolver(schema) });

  const onSubmit = async (data: FormData) => {
    try {
      await AccountService.changePassword({
        email: auth?.email as string,
        currentPassword: data.oldPassword,
        newPassword: data.newPassword,
      });

      toaster.create({
        type: "success",
        title: t("success"),
        description: t("changePassword.toast.success"),
        duration: 3500,
        closable: true,
      });

      setTimeout(() => {
        auth?.logout();
      }, 1500);

      reset();
    } catch (err) {
      toaster.create({
        type: "error",
        title: t("error"),
        description: t("changePassword.toast.error"),
        closable: true,
      });
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
        <Heading fontSize={"2xl"}>{t("changePassword.title")}</Heading>
      </Flex>
      <form onSubmit={handleSubmit(onSubmit)}>
        <Stack padding={5} align="flex-start">
          <Field.Root>
            <Field.Label>{t("changePassword.old")}</Field.Label>
            <PasswordInput {...register("oldPassword")} />
            {errors.oldPassword && (
              <Text color="red">
                {getFormErrorMessage(errors.oldPassword.message)}
              </Text>
            )}
          </Field.Root>
          <Field.Root>
            <Field.Label>{t("changePassword.new")}</Field.Label>
            <PasswordInput {...register("newPassword")} />
            {errors.newPassword && (
              <Text color="red">
                {getFormErrorMessage(errors.newPassword.message)}
              </Text>
            )}{" "}
          </Field.Root>
          <Field.Root>
            <Field.Label>{t("changePassword.confirmNew")}</Field.Label>
            <PasswordInput {...register("confirmNewPassword")} />
            {errors.confirmNewPassword && (
              <Text color="red">
                {getFormErrorMessage(errors.confirmNewPassword.message)}
              </Text>
            )}{" "}
          </Field.Root>
        </Stack>
        <Flex w="100%" justifyContent={"center"} padding={3}>
          <Button
            type="submit"
            background={"green.400"}
            size="lg"
            disabled={isSubmitting}
          >
            {t("changePassword.button")}
          </Button>
        </Flex>
      </form>
    </Box>
  );
};
