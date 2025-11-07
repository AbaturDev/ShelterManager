import { useParams } from "react-router-dom";
import {
  EventDetailsCard,
  EventDetailsHeader,
} from "../components/events/details";
import { useEventDetailsQuery } from "../hooks/useEventDetailsQuery";
import { Text } from "@chakra-ui/react";
import { useTranslation } from "react-i18next";

export const EventsDetailsPage = () => {
  const { t } = useTranslation();
  const { id } = useParams();

  const eventId = id as string;

  const { data, isLoading, error } = useEventDetailsQuery(eventId);

  if (error) return <Text color="red">{t("events.details.error")}</Text>;

  return (
    <>
      <EventDetailsHeader event={data} isLoading={isLoading} />
      <EventDetailsCard event={data} isLoading={isLoading} />
    </>
  );
};
