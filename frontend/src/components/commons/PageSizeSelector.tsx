import {
  createListCollection,
  HStack,
  Select,
  Text,
  Portal,
} from "@chakra-ui/react";

interface PageSizeSelectorProps {
  pageSize: number;
  onPageSizeChange: (pageSize: number) => void;
  pageSizeOptions?: number[];
}

export const PageSizeSelector = ({
  pageSize,
  onPageSizeChange,
  pageSizeOptions = [10, 25, 50, 100],
}: PageSizeSelectorProps) => {
  const collection = createListCollection({
    items: pageSizeOptions.map((size) => ({
      label: size,
      value: size,
    })),
  });

  return (
    <Select.Root
      collection={collection}
      value={[pageSize.toString()]}
      onValueChange={(e) => onPageSizeChange(Number(e.value[0]))}
      positioning={{ placement: "bottom-end" }}
    >
      <Select.HiddenSelect />
      <Select.Control width="fit-content" minW="32">
        <Select.Trigger>
          <HStack justify="space-between" width="full" px="2">
            <Text>{pageSize}</Text>
            <Select.Indicator />
          </HStack>
        </Select.Trigger>
      </Select.Control>

      <Portal>
        <Select.Positioner>
          <Select.Content>
            {collection.items.map((item) => (
              <Select.Item
                item={item}
                key={item.value}
                _hover={{ bg: "green.600", color: "white" }}
              >
                <Text>{item.label}</Text>
                <Select.ItemIndicator />
              </Select.Item>
            ))}
          </Select.Content>
        </Select.Positioner>
      </Portal>
    </Select.Root>
  );
};
