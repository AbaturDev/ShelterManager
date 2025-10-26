import { useSearchParams } from "react-router-dom";
import { ResetPasswordForm } from "../components/password";
import { Flex } from "@chakra-ui/react";

export const ResetPasswordPage = () => {
  const [searchParams] = useSearchParams();

  const rawToken = searchParams.get("token");
  const email = searchParams.get("email");

  const token = rawToken ? encodeURIComponent(rawToken) : null;

  return (
    <Flex justify={"center"} align={"center"}>
      <ResetPasswordForm token={token} email={email} />
    </Flex>
  );
};
