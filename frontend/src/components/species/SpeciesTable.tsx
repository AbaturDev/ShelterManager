import { Table, Stack, Box, Menu, Icon, Portal, Text } from "@chakra-ui/react";
import { Loading, PaginationFooter } from "../commons";
import { useSpeciesListQuery } from "../../hooks/useSpeciesListQuery";
import { useState } from "react";
import { SlOptionsVertical } from "react-icons/sl";
import { FaEye } from "react-icons/fa";
import { useTranslation } from "react-i18next";
import { useNavigate } from "react-router-dom";
import { DeleteSpeciesDialog } from "./DeleteSpeciesDialog";
import { MdDelete } from "react-icons/md";

export const SpeciesTable = () => {
  const { t } = useTranslation();
  const navigate = useNavigate();

  const [page, setPage] = useState(1);
  const [pageSize, setPageSize] = useState(10);
  const [isDeleteOpen, setIsDeleteOpen] = useState(false);
  const [deleteSpeciesId, setDeleteSpeciesId] = useState<string | null>(null);

  const { data, isLoading, error } = useSpeciesListQuery(page, pageSize);

  if (isLoading) return <Loading />;
  if (error) return <Text>{t("species.error")}</Text>;

  const items = data?.items || [];

  return (
    <>
      <Stack width="full" gap="5" paddingY={3}>
        <Table.Root size="lg" variant="outline" interactive>
          <Table.Header>
            <Table.Row>
              <Table.ColumnHeader width={"44%"}>
                {t("species.name")}
              </Table.ColumnHeader>
              <Table.ColumnHeader width={"44%"}>
                {t("species.createdAt")}
              </Table.ColumnHeader>
              <Table.ColumnHeader width={"2%"} textAlign={"center"}>
                {t("species.actions")}
              </Table.ColumnHeader>
            </Table.Row>
          </Table.Header>
          <Table.Body>
            {items.map((item) => (
              <Table.Row
                key={item.id}
                onDoubleClick={() => navigate(`/species/${item.id}`)}
                _hover={{ cursor: "pointer" }}
              >
                <Table.Cell width={"40%"}>{item.name}</Table.Cell>
                <Table.Cell width={"40%"}>
                  {new Date(item.createdAt).toLocaleString()}
                </Table.Cell>
                <Table.Cell textAlign={"center"}>
                  <Menu.Root>
                    <Menu.Trigger
                      focusRing={"outside"}
                      _hover={{ cursor: "pointer" }}
                    >
                      <Icon as={SlOptionsVertical} />
                    </Menu.Trigger>
                    <Portal>
                      <Menu.Positioner>
                        <Menu.Content>
                          <Menu.Item
                            value="details"
                            _hover={{ bg: "green.600", color: "white" }}
                            onSelect={() => navigate(`/species/${item.id}`)}
                          >
                            <Icon as={FaEye} />
                            <Box flex={"1"}>{t("species.details")}</Box>
                          </Menu.Item>
                          <Menu.Item
                            value="delete"
                            _hover={{ bg: "green.600", color: "white" }}
                            onSelect={() => {
                              setIsDeleteOpen(true);
                              setDeleteSpeciesId(item.id);
                            }}
                          >
                            <Icon as={MdDelete} />
                            <span>{t("species.delete")}</span>
                          </Menu.Item>
                        </Menu.Content>
                      </Menu.Positioner>
                    </Portal>
                  </Menu.Root>
                </Table.Cell>
              </Table.Row>
            ))}
          </Table.Body>
        </Table.Root>
        <PaginationFooter
          page={page}
          pageSize={pageSize}
          totalItemsCount={data?.totalItemsCount || 0}
          onPageChange={setPage}
          onPageSizeChange={(size) => {
            setPageSize(size);
            setPage(1);
          }}
        />
      </Stack>
      {deleteSpeciesId && (
        <DeleteSpeciesDialog
          id={deleteSpeciesId}
          isOpen={isDeleteOpen}
          onClose={() => {
            setIsDeleteOpen(false);
            setDeleteSpeciesId(null);
          }}
        />
      )}
    </>
  );
};
