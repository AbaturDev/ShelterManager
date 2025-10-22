import { useForm } from "react-hook-form";
import {
  Button,
  Field,
  Input,
  Link,
  Stack,
  VStack,
  Text,
} from "@chakra-ui/react";
import { PasswordInput } from "../ui/password-input";
import { useTranslation } from "react-i18next";
import z from "zod";
import { zodResolver } from "@hookform/resolvers/zod";
import { AccountService } from "../../api/services/account-service";
import {
  getFormErrorMessage,
  setFormErrorMessage,
} from "../../utils/form-error-handlers";
import { useNavigate } from "react-router-dom";
import { toaster } from "../ui/toaster";
import { AxiosError } from "axios";

const loginSchema = z.object({
  email: z.string().email(setFormErrorMessage("login.emailError")),
  password: z.string().min(8, setFormErrorMessage("login.passwordError")),
});

type LoginData = z.infer<typeof loginSchema>;

export const LoginForm = () => {
  const { t } = useTranslation();
  const navigate = useNavigate();

  const {
    register,
    handleSubmit,
    formState: { isSubmitting, errors },
  } = useForm<LoginData>({
    resolver: zodResolver(loginSchema),
    mode: "onChange",
  });

  const onSubmit = async (data: LoginData) => {
    try {
      const response = await AccountService.login({
        email: data.email,
        password: data.password,
      });

      localStorage.setItem("jwtToken", response.jwtToken);
      localStorage.setItem("refreshToken", response.refreshToken);

      navigate(`/`);
    } catch (err) {
      let message: string | null = null;
      if (err instanceof AxiosError) {
        switch (err.status) {
          case 400:
            message = t("login.error");
            break;
          case 503:
            message = t("errorTooManyRequests");
            break;
          default:
            break;
        }
      }

      toaster.create({
        title: t("error"),
        description: message ?? t("login.error"),
        type: "error",
        closable: true,
      });
    }
  };

  return (
    <form onSubmit={handleSubmit(onSubmit)}>
      <Stack gap="4" align="flex-start" maxW="sm">
        <Field.Root>
          <Field.Label>{t("login.email")}</Field.Label>
          <Input type="email" {...register("email")} />
          {errors.email && (
            <Text color="red">{getFormErrorMessage(errors.email.message)}</Text>
          )}
        </Field.Root>

        <Field.Root>
          <Field.Label>{t("login.password")}</Field.Label>
          <PasswordInput {...register("password")} />
          {errors.password && (
            <Text color="red">
              {getFormErrorMessage(errors.password.message)}
            </Text>
          )}
        </Field.Root>
        <Link fontSize={"sm"} color={"green.400"} href="/forgot-password">
          {t("login.forgotPassword")}
        </Link>
        <VStack width={"100%"} justifyContent={"center"}>
          <Button
            type="submit"
            background={"green.400"}
            size={"lg"}
            disabled={isSubmitting}
          >
            {t("login.login")}
          </Button>
        </VStack>
      </Stack>
    </form>
  );
};
