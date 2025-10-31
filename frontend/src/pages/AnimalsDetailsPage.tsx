import { useParams } from "react-router-dom";
import {
  AnimalsDetailsCard,
  AnimalsDetailsHeader,
} from "../components/animals/details";

export const AnimalsDetailsPage = () => {
  const { id } = useParams();

  const animalId = id as string;

  return (
    <>
      <AnimalsDetailsHeader id={animalId} />
      <AnimalsDetailsCard id={animalId} />
    </>
  );
};
