import { useTranslation } from "react-i18next";
import { useStatisticsQuery } from "../../hooks/useStatisticsQuery";
import { SimpleGrid, Spinner } from "@chakra-ui/react";
import { MdPets, MdEvent, MdFavorite } from "react-icons/md";
import { StatCard } from ".";

export const Dashboard = () => {
  const { t } = useTranslation();
  const { data, isLoading, error } = useStatisticsQuery();

  return (
    <SimpleGrid
      columns={{ base: 1, sm: 2, md: 3 }}
      w="full"
      justifyItems="center"
      gap={10}
    >
      <StatCard
        icon={MdPets}
        label={t("homepage.dashboard.animals")}
        value={
          isLoading ? (
            <Spinner size="md" />
          ) : error ? (
            t("homepage.dashboard.error")
          ) : (
            data?.animalsCount ?? 0
          )
        }
        color="teal.400"
      />

      <StatCard
        icon={MdFavorite}
        label={t("homepage.dashboard.adoptions")}
        value={
          isLoading ? (
            <Spinner size="md" />
          ) : error ? (
            t("homepage.dashboard.error")
          ) : (
            data?.currentAdoptionProcessCount ?? 0
          )
        }
        color="orange.400"
      />

      <StatCard
        icon={MdEvent}
        label={t("homepage.dashboard.events")}
        value={
          isLoading ? (
            <Spinner size="md" />
          ) : error ? (
            t("homepage.dashboard.error")
          ) : (
            data?.upcomingEventsCount ?? 0
          )
        }
        color="purple.400"
      />
    </SimpleGrid>
  );
};
