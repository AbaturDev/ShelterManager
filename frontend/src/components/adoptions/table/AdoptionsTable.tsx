import { Flex, Icon, Menu, Portal, Stack, Table, Text } from "@chakra-ui/react";
import { useAdoptionsQuery } from "../../../hooks/useAdoptionsQuery";
import { useTranslation } from "react-i18next";
import { useNavigate, useSearchParams } from "react-router-dom";
import { Loading, PaginationFooter } from "../../commons";
import { useState } from "react";
import { type AdoptionStatus } from "../../../models/adoptions";
import { SlOptionsVertical } from "react-icons/sl";
import { FaEye } from "react-icons/fa";
import { MdDelete } from "react-icons/md";
import { DeleteAdoptionDialog } from "../DeleteAdoptionDialog";
import { AdoptionStatusBadge } from "../AdoptionStatusBadge";

interface AdoptionsTableProps {
  search?: string;
}

export const AdoptionsTable = ({ search }: AdoptionsTableProps) => {
  const { t } = useTranslation();
  const navigate = useNavigate();

  const [deleteAdoptionId, setDeleteAdoptionId] = useState<string | null>(null);
  const [isDeleteOpen, setIsDeleteOpen] = useState(false);
  const [page, setPage] = useState(1);
  const [pageSize, setPageSize] = useState(10);

  const [searchParams] = useSearchParams();
  const animalIdsParam = searchParams.getAll("animalIds");
  const statusParam = searchParams.getAll("status");

  const { data, isLoading, error } = useAdoptionsQuery({
    page: page,
    pageSize: pageSize,
    animalName: search,
    animalIds: animalIdsParam ?? undefined,
    status: statusParam.map((s) => Number(s) as AdoptionStatus),
  });

  if (isLoading) return <Loading />;
  if (error || data === undefined) {
    return <Text color="red">{t("adoptions.list.error")}</Text>;
  }

  return (
    <>
      <Flex w="100%" justifyContent={"center"} marginY={5}>
        <Stack width="80%" gap="5">
          <Table.Root size="lg" variant="outline" interactive>
            <Table.Header>
              <Table.Row>
                <Table.ColumnHeader textAlign={"center"} width={"28%"}>
                  {t("adoptions.list.animal")}
                </Table.ColumnHeader>
                <Table.ColumnHeader textAlign={"center"} width={"10%"}>
                  {t("adoptions.list.status")}
                </Table.ColumnHeader>
                <Table.ColumnHeader width={"29%"} textAlign={"center"}>
                  {t("adoptions.list.startDate")}
                </Table.ColumnHeader>
                <Table.ColumnHeader width={"29%"} textAlign={"center"}>
                  {t("adoptions.list.adoptionDate")}
                </Table.ColumnHeader>
                <Table.ColumnHeader width={"4%"} textAlign={"center"}>
                  {t("adoptions.list.actions")}
                </Table.ColumnHeader>
              </Table.Row>
            </Table.Header>
            <Table.Body>
              {data.items.map((item) => (
                <Table.Row
                  key={item.id}
                  onDoubleClick={() => navigate(`/adoptions/${item.id}`)}
                >
                  <Table.Cell textAlign={"center"} width={"28%"}>
                    {item.animalName}
                  </Table.Cell>
                  <Table.Cell width={"10%"} textAlign={"center"}>
                    <AdoptionStatusBadge status={item.status} />
                  </Table.Cell>
                  <Table.Cell textAlign={"center"} width={"29%"}>
                    {new Date(item.startAdoptionProcess).toLocaleString()}
                  </Table.Cell>{" "}
                  <Table.Cell textAlign={"center"} width={"29%"}>
                    {item.adoptionDate !== undefined
                      ? new Date(item.adoptionDate).toLocaleString()
                      : "---"}
                  </Table.Cell>{" "}
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
                              _hover={{
                                bg: "green.600",
                                color: "white",
                                cursor: "pointer",
                              }}
                              onSelect={() => navigate(`/adoptions/${item.id}`)}
                            >
                              <Icon as={FaEye} />
                              <span>{t("details")}</span>
                            </Menu.Item>
                            <Menu.Item
                              value="delete"
                              _hover={{
                                bg: "green.600",
                                color: "white",
                                cursor: "pointer",
                              }}
                              onSelect={() => {
                                setIsDeleteOpen(true);
                                setDeleteAdoptionId(item.id);
                              }}
                            >
                              <Icon as={MdDelete} />
                              <span>{t("delete")}</span>
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
            totalItemsCount={data.totalItemsCount}
            onPageChange={setPage}
            onPageSizeChange={(size) => {
              setPageSize(size);
              setPage(1);
            }}
            pageSizeOptions={[10, 25, 50, 100]}
          />
        </Stack>
      </Flex>
      {deleteAdoptionId && (
        <DeleteAdoptionDialog
          id={deleteAdoptionId}
          isOpen={isDeleteOpen}
          onClose={() => {
            setIsDeleteOpen(false);
            setDeleteAdoptionId(null);
          }}
          onSuccess={() => {
            setIsDeleteOpen(false);
            setDeleteAdoptionId(null);
          }}
        />
      )}
    </>
  );
};
