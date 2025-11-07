import {
  Button,
  Checkbox,
  Flex,
  Heading,
  HStack,
  Icon,
  Menu,
  VStack,
} from "@chakra-ui/react";
import { useTranslation } from "react-i18next";
import { AddEventDialog } from "./AddEventDialog";
import { useAnimalsListQuery } from "../../../hooks/useAnimalsListQuery";
import { IoMdArrowDropdown } from "react-icons/io";
import { FaFilter } from "react-icons/fa";
import { useSearchParams } from "react-router-dom";
import { useEffect, useState } from "react";

export const EventsListHeader = () => {
  const { t } = useTranslation();
  const [searchParams, setSearchParams] = useSearchParams();
  const { data: animals } = useAnimalsListQuery({
    page: 1,
    pageSize: 10000000,
  });

  const [selectedAnimals, setSelectedAnimals] = useState<string[]>([]);
  const [status, setStatus] = useState<boolean | null>(null);

  useEffect(() => {
    const animalIdsParam = searchParams.getAll("animalIds");
    const statusParam = searchParams.get("status");

    if (animalIdsParam) setSelectedAnimals(animalIdsParam);
    if (statusParam === "true") setStatus(true);
    else if (statusParam === "false") setStatus(false);
    else setStatus(null);
  }, [searchParams]);

  const handleAnimalToggle = (id: string, checked: boolean) => {
    setSelectedAnimals((prev) =>
      checked ? [...prev, id] : prev.filter((a) => a !== id)
    );
  };

  const handleFilter = () => {
    const params = new URLSearchParams();

    selectedAnimals.forEach((id) => {
      params.append("animalIds", id);
    });

    if (status !== null) params.append("status", String(status));
    setSearchParams(params);
  };

  return (
    <Flex direction="column" gap={7}>
      <Heading size="3xl">{t("events.list.header")}</Heading>

      <Flex w="100%" justifyContent="center">
        <HStack w="80%" justifyContent="space-between">
          <AddEventDialog />

          <HStack>
            <Menu.Root>
              <Menu.Trigger asChild>
                <Button bg="gray.400" gap={1}>
                  <Icon as={IoMdArrowDropdown} /> {t("events.list.animals")}
                </Button>
              </Menu.Trigger>
              <Menu.Positioner>
                <Menu.Content>
                  <VStack align="flex-start" p={2}>
                    {animals?.items.map((animal) => (
                      <Checkbox.Root
                        key={animal.id}
                        colorPalette={"green"}
                        checked={selectedAnimals.includes(animal.id)}
                        onCheckedChange={({ checked }) =>
                          handleAnimalToggle(animal.id, checked === true)
                        }
                      >
                        <Checkbox.HiddenInput />
                        <Checkbox.Control />
                        <Checkbox.Label>{animal.name}</Checkbox.Label>
                      </Checkbox.Root>
                    ))}
                  </VStack>
                </Menu.Content>
              </Menu.Positioner>
            </Menu.Root>

            <Menu.Root>
              <Menu.Trigger asChild>
                <Button bg="gray.400" gap={1}>
                  <Icon as={IoMdArrowDropdown} /> {t("events.list.status")}
                </Button>
              </Menu.Trigger>
              <Menu.Positioner>
                <Menu.Content>
                  <VStack p={2}>
                    <Button
                      w="100%"
                      variant={status === false ? "solid" : "ghost"}
                      background={status === false ? "green.600" : "white"}
                      onClick={() =>
                        setStatus((prev) => (prev === false ? null : false))
                      }
                    >
                      {t("events.list.active")}
                    </Button>
                    <Button
                      w="100%"
                      variant={status === true ? "solid" : "ghost"}
                      background={status === true ? "green.600" : "white"}
                      onClick={() =>
                        setStatus((prev) => (prev === true ? null : true))
                      }
                    >
                      {t("events.list.completed")}
                    </Button>
                  </VStack>
                </Menu.Content>
              </Menu.Positioner>
            </Menu.Root>

            <Button onClick={handleFilter}>
              <Icon as={FaFilter} boxSize={4} /> {t("events.list.filter")}
            </Button>
          </HStack>
        </HStack>
      </Flex>
    </Flex>
  );
};
