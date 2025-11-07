import { Input, InputGroup } from "@chakra-ui/react";
import { useRef } from "react";
import { useTranslation } from "react-i18next";
import { BsSearch } from "react-icons/bs";

interface Props {
  search?: string;
  onSearch: (searchText: string) => void;
}

export const SearchInput = ({ search, onSearch }: Props) => {
  const { t } = useTranslation();
  const ref = useRef<HTMLInputElement>(null);
  const timeoutRef = useRef<number | null>(null);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const value = e.target.value;

    if (timeoutRef.current) {
      clearTimeout(timeoutRef.current);
    }

    timeoutRef.current = window.setTimeout(() => {
      onSearch(value);
    }, 300);
  };

  return (
    <InputGroup startElement={<BsSearch color="gray.500" />}>
      <Input
        ref={ref}
        ps="2.5em"
        defaultValue={search}
        borderRadius={20}
        placeholder={t("search")}
        variant="subtle"
        onChange={handleChange}
      />
    </InputGroup>
  );
};
