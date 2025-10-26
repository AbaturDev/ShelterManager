import { Flex } from "@chakra-ui/react";
import { LoginForm } from "../components/login";

export const LoginPage = () => {
  return (
    <Flex justify={"center"} align={"center"}>
      <LoginForm />
    </Flex>
  );
};
