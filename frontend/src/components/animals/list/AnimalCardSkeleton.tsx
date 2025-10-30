import { Card, Skeleton, SkeletonText } from "@chakra-ui/react";

export const AnimalCardSkeleton = () => {
  return (
    <Card.Root>
      <Skeleton height="200px" />
      <Card.Body>
        <SkeletonText />
      </Card.Body>
    </Card.Root>
  );
};
