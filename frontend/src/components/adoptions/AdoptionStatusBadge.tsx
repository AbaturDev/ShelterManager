import { Badge } from "@chakra-ui/react";
import {
  PossibleAdoptionStatus,
  type AdoptionStatus,
} from "../../models/adoptions";
import { useTranslation } from "react-i18next";

interface Props {
  status: AdoptionStatus;
}

export const AdoptionStatusBadge = ({ status }: Props) => {
  const { t } = useTranslation();

  switch (status) {
    case PossibleAdoptionStatus.Active:
      return (
        <Badge colorPalette="yellow">{t("adoptions.status.active")}</Badge>
      );
    case PossibleAdoptionStatus.Approved:
      return (
        <Badge colorPalette="green">{t("adoptions.status.approved")}</Badge>
      );
    case PossibleAdoptionStatus.Rejected:
      return <Badge colorPalette="red">{t("adoptions.status.rejected")}</Badge>;
    default:
      return "-";
  }
};
