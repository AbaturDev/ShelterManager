import {
  HStack,
  IconButton,
  Portal,
  Select,
  createListCollection,
  useSelectContext,
  Text,
} from "@chakra-ui/react";
import { useTranslation } from "react-i18next";
import { FlagEnIcon, FlagPlIcon } from "../../assets/flags";

interface Language {
  value: string;
  label: string;
  flag: React.ReactNode;
}

const LanguageSelectTrigger = () => {
  const select = useSelectContext();
  const items = select.selectedItems as Language[];
  const selected = items[0];

  return (
    <IconButton px="2" variant="ghost" size="md" {...select.getTriggerProps()}>
      {selected ? (
        <HStack>
          {selected.flag}
          <Text>{selected.label}</Text>
        </HStack>
      ) : (
        <Text>?</Text>
      )}
    </IconButton>
  );
};

export const LanguageSelector = () => {
  const { i18n } = useTranslation();

  const languages = createListCollection({
    items: [
      { value: "pl", label: "PL", flag: <FlagPlIcon /> },
      { value: "en", label: "EN", flag: <FlagEnIcon /> },
    ],
  });

  const onLanguageSelect = (language: string) => {
    i18n.changeLanguage(language);
  };

  return (
    <Select.Root
      collection={languages}
      defaultValue={["pl"]}
      onValueChange={(e) => onLanguageSelect(e.value[0])}
    >
      <Select.HiddenSelect />
      <Select.Control>
        <LanguageSelectTrigger />
      </Select.Control>
      <Portal>
        <Select.Positioner>
          <Select.Content minW="32">
            {languages.items.map((lang) => (
              <Select.Item
                item={lang}
                key={lang.value}
                _hover={{ bg: "green.600", color: "white" }}
              >
                <HStack>
                  {lang.flag}
                  <Text>{lang.label}</Text>
                </HStack>
                <Select.ItemIndicator />
              </Select.Item>
            ))}
          </Select.Content>
        </Select.Positioner>
      </Portal>
    </Select.Root>
  );
};
