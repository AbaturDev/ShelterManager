import { useState } from "react";
import { EventsListHeader, EventsTable } from "../components/events/table";

export const EventsPage = () => {
  const [search, setSearch] = useState<string | undefined>(undefined);

  return (
    <>
      <EventsListHeader onSearch={setSearch} />
      <EventsTable search={search} />
    </>
  );
};
