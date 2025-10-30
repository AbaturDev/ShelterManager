import { Stack, Table, Text, Icon } from "@chakra-ui/react";
import { useUsersQuery } from "../../../hooks/useUsersQuery";
import { useState } from "react";
import { useTranslation } from "react-i18next";
import { Loading, PaginationFooter } from "../../commons";
import { MdDelete } from "react-icons/md";
import { DeleteUserDialog } from ".";
import { UserPopover } from "./UserPopover";

export const UsersTable = () => {
  const { t } = useTranslation();

  const [page, setPage] = useState(1);
  const [pageSize, setPageSize] = useState(10);
  const [isDeleteOpen, setIsDeleteOpen] = useState(false);
  const [deleteUserId, setDeleteUserId] = useState<string | null>(null);
  const [isDetailsOpen, setIsDetailsOpen] = useState(false);
  const [detailsUserId, setDetailsUserId] = useState<string | null>(null);
  const [detailsAnchorEl, setDetailsAnchorEl] = useState<HTMLElement | null>(
    null
  );

  const { data, isLoading, error } = useUsersQuery({
    page: page,
    pageSize: pageSize,
  });

  if (isLoading) return <Loading />;
  if (error) return <Text color={"red"}>{t("user.list.error")}</Text>;

  const items = data?.items || [];

  return (
    <>
      <Stack width="full" gap="5" paddingY={3}>
        <Table.Root size="lg" variant="outline" interactive>
          <Table.Header>
            <Table.Row>
              <Table.ColumnHeader width={"32%"}>
                {t("user.list.name")}
              </Table.ColumnHeader>
              <Table.ColumnHeader width={"32%"}>
                {t("user.list.surname")}
              </Table.ColumnHeader>
              <Table.ColumnHeader width={"32%"}>
                {t("user.list.email")}
              </Table.ColumnHeader>
              <Table.ColumnHeader width={"4%"} textAlign={"center"} />
            </Table.Row>
          </Table.Header>
          <Table.Body>
            {items.map((item) => (
              <Table.Row
                key={item.id}
                onDoubleClick={(e) => {
                  setIsDetailsOpen(true);
                  setDetailsUserId(item.id);
                  setDetailsAnchorEl(e.currentTarget);
                }}
                bg={detailsUserId === item.id ? "green.100" : undefined}
              >
                <Table.Cell width={"32%"}>{item.name}</Table.Cell>
                <Table.Cell width={"32%"}>{item.surname}</Table.Cell>
                <Table.Cell width={"32%"}>{item.email}</Table.Cell>
                <Table.Cell textAlign={"center"}>
                  <Icon
                    as={MdDelete}
                    boxSize={6}
                    _hover={{ cursor: "pointer" }}
                    onClick={() => {
                      setDeleteUserId(item.id);
                      setIsDeleteOpen(true);
                    }}
                  />
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
          pageSizeOptions={[10, 25, 50, 100]}
        />
      </Stack>
      {deleteUserId && (
        <DeleteUserDialog
          id={deleteUserId}
          isOpen={isDeleteOpen}
          onClose={() => {
            setIsDeleteOpen(false);
            setDeleteUserId(null);
          }}
          onSuccess={() => {
            setIsDeleteOpen(false);
            setDeleteUserId(null);
          }}
        />
      )}
      {detailsUserId && (
        <UserPopover
          userId={detailsUserId}
          isOpen={isDetailsOpen}
          onClose={() => {
            setIsDetailsOpen(false);
            setDetailsUserId(null);
            setDetailsAnchorEl(null);
          }}
          anchorEl={detailsAnchorEl}
        />
      )}
    </>
  );
};
