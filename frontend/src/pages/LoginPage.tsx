import { Box, Flex } from "@chakra-ui/react";
import { LoginForm } from "../components/login";

export const LoginPage = () => {
  return (
    <Flex justify={"center"} align={"center"} minH="40vh">
      <Box
        p={8}
        borderWidth="1px"
        borderRadius="md"
        boxShadow="md"
        bg="white"
        minW="sm"
      >
        <LoginForm />
      </Box>
    </Flex>
  );
};
