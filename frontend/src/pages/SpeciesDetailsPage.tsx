import { useParams } from "react-router-dom";
import { SpeciesDetails } from "../components/speciesDetails";
import { BackButton } from "../components/commons";

export const SpeciesDetailsPage = () => {
  const { id } = useParams();

  return (
    <>
      <BackButton />
      <SpeciesDetails id={id as string} />
    </>
  );
};
