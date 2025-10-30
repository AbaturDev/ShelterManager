import { Box } from "@chakra-ui/react";
import type { ReactNode } from "react";

interface AnimalCardContainerProps {
  children: ReactNode;
}

export const AnimalCardContainer = ({ children }: AnimalCardContainerProps) => {
  return (
    <Box borderRadius={20} overflow="hidden" padding={1} height="350px">
      {children}
    </Box>
  );
};
