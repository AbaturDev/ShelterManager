import { Input, InputGroup } from "@chakra-ui/react";
import { useRef } from "react";
import { useTranslation } from "react-i18next";
import { BsSearch } from "react-icons/bs";

interface Props {
  onSearch: (searchText: string) => void;
}

export const SearchInput = ({ onSearch }: Props) => {
  const { t } = useTranslation();
  const ref = useRef<HTMLInputElement>(null);

  return (
    <form
      onSubmit={(event) => {
        event.preventDefault();
        if (ref.current) {
          onSearch(ref.current.value);
        }
      }}
    >
      <InputGroup startElement={<BsSearch color="gray.500" />}>
        <Input
          ref={ref}
          ps="2.5em"
          borderRadius={20}
          placeholder={t("search")}
          variant="subtle"
        />
      </InputGroup>
    </form>
  );
};
