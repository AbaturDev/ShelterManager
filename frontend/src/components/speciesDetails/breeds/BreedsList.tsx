import { Text, Stack, HStack, Box, Flex, Icon } from "@chakra-ui/react";
import { useTranslation } from "react-i18next";
import { useBreedsListQuery } from "../../../hooks/useBreedsListQuery";
import { Loading, PaginationMenu } from "../../commons";
import { useState } from "react";
import { MdDelete } from "react-icons/md";
import { DeleteBreedDialog } from "./DeleteBreedDialog";

interface BreedsListProps {
  speciesId: string;
}

const breedListPageSize = 5;

export const BreedsList = ({ speciesId }: BreedsListProps) => {
  const { t } = useTranslation();
  const [isDeleteOpen, setIsDeleteOpen] = useState(false);
  const [deleteBreedId, setDeleteBreedId] = useState<string | null>(null);
  const [page, setPage] = useState(1);
  const { data, isLoading, error } = useBreedsListQuery(
    speciesId,
    page,
    breedListPageSize
  );

  if (isLoading) return <Loading />;
  if (error) return <Text>{t("breeds.error")}</Text>;

  if (data?.items === null || data?.items.length === 0)
    return <Text>{t("breeds.empty")}</Text>;

  const breeds = data?.items || [];

  return (
    <>
      <Stack>
        {breeds.map((breed) => (
          <HStack
            key={breed.id}
            justify="space-between"
            p={3}
            borderWidth="1px"
            borderRadius="md"
            _hover={{ bg: "gray.50" }}
          >
            <Box>
              <Text fontWeight="medium">{breed.name}</Text>
              <Text fontSize="sm" color="gray.500">
                {t("createdAt")}:{" "}
                {new Date(breed.createdAt).toLocaleDateString()}
              </Text>
            </Box>
            <Icon
              as={MdDelete}
              boxSize={6}
              _hover={{ cursor: "pointer" }}
              onClick={() => {
                setDeleteBreedId(breed.id);
                setIsDeleteOpen(true);
              }}
            />
          </HStack>
        ))}
      </Stack>
      <Flex w="100%" align={"center"} justify={"center"}>
        <PaginationMenu
          page={page}
          pageSize={breedListPageSize}
          onPageChange={setPage}
          totalItemsCount={data?.totalItemsCount!}
        />
      </Flex>
      {deleteBreedId && (
        <DeleteBreedDialog
          id={deleteBreedId}
          isOpen={isDeleteOpen}
          speciesId={speciesId}
          onClose={() => {
            setIsDeleteOpen(false);
            setDeleteBreedId(null);
          }}
          onSuccess={() => {
            setIsDeleteOpen(false);
            setDeleteBreedId(null);
          }}
        />
      )}
    </>
  );
};
