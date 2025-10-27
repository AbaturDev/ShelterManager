import { useState } from "react";
import { AnimalGrid, Header } from "../components/animals/list";

export const AnimalsPage = () => {
  const [search, setSearch] = useState<string | undefined>(undefined);

  return (
    <>
      <Header onSearch={setSearch} />
      <AnimalGrid search={search} />
    </>
  );
};
