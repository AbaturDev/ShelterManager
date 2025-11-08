import {
  Box,
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
import { useAnimalsListQuery } from "../../../hooks/useAnimalsListQuery";
import { IoMdArrowDropdown } from "react-icons/io";
import { FaFilter } from "react-icons/fa";
import { useSearchParams } from "react-router-dom";
import { useEffect, useState } from "react";
import { SearchInput } from "../../commons";
import {
  PossibleAdoptionStatus,
  type AdoptionStatus,
} from "../../../models/adoptions";
import { AddAdoptionDialog } from "../create";

interface AdoptionsHeaderProps {
  onSearch: (query: string) => void;
}

export const AdoptionsHeader = ({ onSearch }: AdoptionsHeaderProps) => {
  const { t } = useTranslation();
  const [searchParams, setSearchParams] = useSearchParams();
  const { data: animals } = useAnimalsListQuery({
    page: 1,
    pageSize: 10000000,
  });

  const [selectedAnimals, setSelectedAnimals] = useState<string[]>([]);
  const [selectedStatus, setSelectedStatus] = useState<AdoptionStatus[]>([]);

  useEffect(() => {
    const animalIdsParam = searchParams.getAll("animalIds");
    const statusParam = searchParams.getAll("status");

    if (animalIdsParam) setSelectedAnimals(animalIdsParam);
    if (statusParam)
      setSelectedStatus(statusParam.map((s) => Number(s) as AdoptionStatus));
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

    selectedStatus.forEach((status) => {
      params.append("status", String(status));
    });

    setSearchParams(params);
  };

  return (
    <Flex direction="column" gap={7}>
      <Heading size="3xl">{t("adoptions.list.header")}</Heading>

      <Flex w="100%" justifyContent={"center"}>
        <HStack w="80%" justifyContent="space-between">
          <AddAdoptionDialog />

          <Box flex="1" maxW="700px">
            <SearchInput onSearch={onSearch} />
          </Box>

          <HStack>
            <Menu.Root>
              <Menu.Trigger asChild>
                <Button bg="gray.400" gap={1}>
                  <Icon as={IoMdArrowDropdown} /> {t("adoptions.list.animals")}
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
                  <Icon as={IoMdArrowDropdown} /> {t("adoptions.list.status")}
                </Button>
              </Menu.Trigger>
              <Menu.Positioner>
                <Menu.Content>
                  <VStack align="flex-start" p={2}>
                    {Object.entries(PossibleAdoptionStatus).map(
                      ([key, value]) => (
                        <Checkbox.Root
                          key={key}
                          colorPalette="green"
                          checked={selectedStatus.includes(
                            value as AdoptionStatus
                          )}
                          onCheckedChange={({ checked }) => {
                            setSelectedStatus((prev) => {
                              if (checked === true) {
                                return [...prev, value as AdoptionStatus];
                              } else {
                                return prev.filter((s) => s !== value);
                              }
                            });
                          }}
                        >
                          <Checkbox.HiddenInput />
                          <Checkbox.Control />
                          <Checkbox.Label>
                            {t(`adoptions.status.${key.toLowerCase()}`)}
                          </Checkbox.Label>
                        </Checkbox.Root>
                      )
                    )}
                  </VStack>
                </Menu.Content>
              </Menu.Positioner>
            </Menu.Root>

            <Button onClick={handleFilter}>
              <Icon as={FaFilter} boxSize={4} /> {t("filter")}
            </Button>
          </HStack>
        </HStack>
      </Flex>
    </Flex>
  );
};
