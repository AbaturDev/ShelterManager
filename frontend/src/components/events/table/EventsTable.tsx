import {
  Box,
  Flex,
  Icon,
  Menu,
  Portal,
  Stack,
  Table,
  Text,
} from "@chakra-ui/react";
import { Loading, PaginationFooter } from "../../commons";
import { useState } from "react";
import { useEventsQuery } from "../../../hooks/useEventsQuery";
import { useTranslation } from "react-i18next";
import { useNavigate, useSearchParams } from "react-router-dom";
import { FaEye } from "react-icons/fa";
import { SlOptionsVertical } from "react-icons/sl";
import { MdDelete, MdEdit } from "react-icons/md";
import { DeleteEventDialog } from "../DeleteEventDialog";
import { GrCompliance } from "react-icons/gr";
import { EndEventDialog } from "../EndEventDialog";
import type { Event } from "../../../models/event";
import { EditEventDialog } from "../EditEventDialog";

export const EventsTable = () => {
  const { t } = useTranslation();
  const navigate = useNavigate();
  const [page, setPage] = useState(1);
  const [pageSize, setPageSize] = useState(10);

  const [deleteEventId, setDeleteEventId] = useState<string | null>(null);
  const [isDeleteOpen, setIsDeleteOpen] = useState(false);
  const [endEventId, setEndEventId] = useState<string | null>(null);
  const [isEndOpen, setIsEndOpen] = useState(false);
  const [selectedEvent, setSelectedEvent] = useState<Event | null>(null);
  const [isEditOpen, setIsEditOpen] = useState(false);

  const [searchParams] = useSearchParams();
  const animalIdsParam = searchParams.getAll("animalIds");
  const statusParam = searchParams.get("status");

  console.log(animalIdsParam);

  const { data, isLoading, error } = useEventsQuery({
    page,
    pageSize,
    animalIds: animalIdsParam ?? undefined,
    isDone:
      statusParam === "true"
        ? true
        : statusParam === "false"
        ? false
        : undefined,
  });

  if (isLoading) return <Loading />;
  if (error || data === undefined)
    return <Text color="red">{t("events.list.error")}</Text>;

  return (
    <>
      <Flex w="100%" justifyContent={"center"} marginY={5}>
        <Stack width="80%" gap="5">
          <Table.Root size="lg" variant="outline" interactive>
            <Table.Header>
              <Table.Row>
                <Table.ColumnHeader textAlign={"center"} width={"21%"}>
                  {t("events.list.title")}
                </Table.ColumnHeader>
                <Table.ColumnHeader textAlign={"center"} width={"21%"}>
                  {t("events.list.when")}
                </Table.ColumnHeader>
                <Table.ColumnHeader width={"21%"} textAlign={"center"}>
                  {t("events.list.where")}
                </Table.ColumnHeader>
                <Table.ColumnHeader width={"21%"} textAlign={"center"}>
                  {t("events.list.animal")}
                </Table.ColumnHeader>
                <Table.ColumnHeader width={"11%"} textAlign={"center"}>
                  {t("events.list.isDone")}
                </Table.ColumnHeader>
                <Table.ColumnHeader width={"5%"} textAlign={"center"}>
                  {t("events.list.actions")}
                </Table.ColumnHeader>
              </Table.Row>
            </Table.Header>
            <Table.Body>
              {data.items.map((item) => (
                <Table.Row
                  key={item.id}
                  onDoubleClick={() => navigate(`/events/${item.id}`)}
                >
                  <Table.Cell textAlign={"center"} width={"21%"}>
                    {item.title}
                  </Table.Cell>
                  <Table.Cell width={"21%"} textAlign={"center"}>
                    {new Date(item.date).toLocaleString()}
                  </Table.Cell>
                  <Table.Cell textAlign={"center"} width={"21%"}>
                    {item.location}
                  </Table.Cell>{" "}
                  <Table.Cell textAlign={"center"} width={"21%"}>
                    {item.animalName}
                  </Table.Cell>{" "}
                  <Table.Cell textAlign={"center"} width={"11%"}>
                    {item.isDone === true ? <>{t("yes")}</> : <>{t("no")}</>}
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
                              _hover={{
                                bg: "green.600",
                                color: "white",
                                cursor: "pointer",
                              }}
                              onSelect={() => navigate(`/events/${item.id}`)}
                            >
                              <Icon as={FaEye} />
                              <Box flex={"1"}>{t("events.list.details")}</Box>
                            </Menu.Item>
                            <Menu.Item
                              value="edit"
                              _hover={{
                                bg: "green.600",
                                color: "white",
                                cursor: "pointer",
                              }}
                              onSelect={() => {
                                setIsEditOpen(true);
                                setSelectedEvent(item);
                              }}
                            >
                              <Icon as={MdEdit} />
                              <span>{t("edit")}</span>
                            </Menu.Item>
                            <Menu.Item
                              value="end"
                              disabled={item.isDone}
                              _hover={{
                                bg: "green.600",
                                color: "white",
                                cursor: "pointer",
                              }}
                              onSelect={() => {
                                setIsEndOpen(true);
                                setEndEventId(item.id);
                              }}
                            >
                              <Icon as={GrCompliance} />
                              <span>{t("events.list.end")}</span>
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
                                setDeleteEventId(item.id);
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
      {deleteEventId && (
        <DeleteEventDialog
          id={deleteEventId}
          isOpen={isDeleteOpen}
          onClose={() => {
            setIsDeleteOpen(false);
            setDeleteEventId(null);
          }}
          onSuccess={() => {
            setIsDeleteOpen(false);
            setDeleteEventId(null);
          }}
        />
      )}
      {endEventId && (
        <EndEventDialog
          id={endEventId}
          isOpen={isEndOpen}
          onClose={() => {
            setIsEndOpen(false);
            setEndEventId(null);
          }}
          onSuccess={() => {
            setIsEndOpen(false);
            setEndEventId(null);
          }}
        />
      )}
      {selectedEvent && (
        <EditEventDialog
          event={selectedEvent}
          isOpen={isEditOpen}
          onClose={() => {
            setIsEditOpen(false);
            setSelectedEvent(null);
          }}
          onSuccess={() => {
            setIsEditOpen(false);
            setSelectedEvent(null);
          }}
        />
      )}
    </>
  );
};
