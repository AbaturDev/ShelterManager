import { useParams } from "react-router-dom";
import {
  AdoptionDetailsCard,
  AdoptionDetailsHeader,
} from "../components/adoptions/details";
import { useAdoptionByIdQuery } from "../hooks/useAdoptionByIdQuery";

export const AdoptionsDetailsPage = () => {
  const { id } = useParams();

  const adoptionId = id as string;

  const { data, isLoading, error } = useAdoptionByIdQuery(adoptionId);

  return (
    <>
      <AdoptionDetailsHeader adoption={data} isLoading={isLoading} />
      <AdoptionDetailsCard
        adoption={data}
        isLoading={isLoading}
        error={error}
      />
    </>
  );
};
