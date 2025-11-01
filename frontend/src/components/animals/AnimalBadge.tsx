import { useTranslation } from "react-i18next";
import {
  PossibleAnimalStatus,
  type AnimalStatusType,
} from "../../models/animal";
import { Badge } from "@chakra-ui/react";

interface AnimalBadgeProps {
  status: AnimalStatusType;
}

export const AnimalBadge = ({ status }: AnimalBadgeProps) => {
  const { t } = useTranslation();

  switch (status) {
    case PossibleAnimalStatus.InShelter:
      return (
        <Badge variant={"solid"} colorPalette={"blue"}>
          {t("animals.status.inShelter")}
        </Badge>
      );
    case PossibleAnimalStatus.Adopted:
      return (
        <Badge variant={"solid"} colorPalette={"green"}>
          {t("animals.status.adopted")}
        </Badge>
      );
    case PossibleAnimalStatus.Died:
      return (
        <Badge variant={"solid"} colorScheme={"red"}>
          {t("animals.status.died")}
        </Badge>
      );
  }
};
