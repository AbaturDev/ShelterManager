import { Badge } from "@chakra-ui/react";
import { useTranslation } from "react-i18next";

interface Props {
  isDone: boolean;
}

export const EventStatusBadge = ({ isDone }: Props) => {
  const { t } = useTranslation();

  switch (isDone) {
    case true:
      return (
        <Badge colorPalette={"green"}>{t("events.details.completed")}</Badge>
      );
    case false:
      return (
        <Badge colorPalette={"orange"}>{t("events.details.active")}</Badge>
      );
  }
};
