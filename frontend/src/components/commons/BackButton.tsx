import { Button, Icon } from "@chakra-ui/react";
import { IoMdArrowRoundBack } from "react-icons/io";
import { useNavigate } from "react-router-dom";

export const BackButton = () => {
  const naviage = useNavigate();

  return (
    <Button variant={"ghost"} onClick={() => naviage(-1)}>
      <Icon as={IoMdArrowRoundBack} />
    </Button>
  );
};
