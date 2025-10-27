import { Flex, SimpleGrid, Text } from "@chakra-ui/react";
import { useAnimalsListQuery } from "../../../hooks/useAnimalsListQuery";
import { AnimalCardSkeleton, AnimalCard, AnimalCardContainer } from "./";
import { useTranslation } from "react-i18next";
import { PaginationMenu } from "../../commons";
import { useState } from "react";

interface AnimalGridProps {
  search?: string;
}

export const AnimalGrid = ({ search }: AnimalGridProps) => {
  const [page, setPage] = useState(1);
  const { t } = useTranslation();
  const { data, isLoading, error } = useAnimalsListQuery({
    page: 1,
    pageSize: 10,
    name: search,
  });

  if (error) return <Text color={"red"}>{t("animals.list.error")}</Text>;

  if (data?.items === null || data?.items.length === 0)
    return <Text>{t("animals.list.empty")}</Text>;

  const skeletons = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10];

  const animals = data?.items || [];

  return (
    <>
      <SimpleGrid columns={{ sm: 1, md: 2, lg: 5, xl: 5 }} gap={10}>
        {isLoading &&
          skeletons.map((s) => (
            <AnimalCardContainer key={s}>
              <AnimalCardSkeleton />
            </AnimalCardContainer>
          ))}
        {animals.map((animal) => (
          <AnimalCardContainer key={animal.id}>
            <AnimalCard animal={animal} />
          </AnimalCardContainer>
        ))}
      </SimpleGrid>
      <Flex w="100%" justifyContent={"center"} marginTop={10}>
        <PaginationMenu
          page={page}
          pageSize={10}
          onPageChange={setPage}
          totalItemsCount={data?.totalItemsCount || skeletons.length}
        />
      </Flex>
    </>
  );
};
