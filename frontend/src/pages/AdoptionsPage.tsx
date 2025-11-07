import { useState } from "react";
import { AdoptionsHeader, AdoptionsTable } from "../components/adoptions/table";

export const AdoptionsPage = () => {
  const [search, setSearch] = useState<string | undefined>(undefined);

  return (
    <>
      <AdoptionsHeader onSearch={setSearch} />
      <AdoptionsTable search={search} />
    </>
  );
};
