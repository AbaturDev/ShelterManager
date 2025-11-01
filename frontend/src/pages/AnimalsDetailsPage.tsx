import { useParams } from "react-router-dom";
import {
  AnimalDetailsCard,
  AnimalDetailsHeader,
} from "../components/animals/details";

export const AnimalsDetailsPage = () => {
  const { id } = useParams();

  const animalId = id as string;

  return (
    <>
      <AnimalDetailsHeader id={animalId} />
      <AnimalDetailsCard id={animalId} />
    </>
  );
};
