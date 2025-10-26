import { useParams } from "react-router-dom";
import { SpeciesDetails } from "../components/speciesDetails";

export const SpeciesDetailsPage = () => {
  const { id } = useParams();

  return (
    <>
      <SpeciesDetails id={id as string} />
    </>
  );
};
