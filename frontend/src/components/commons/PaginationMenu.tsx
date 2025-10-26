import {
  Pagination,
  IconButton,
  ButtonGroup,
  type PaginationPageChangeDetails,
} from "@chakra-ui/react";
import { LuChevronLeft, LuChevronRight } from "react-icons/lu";

interface PaginationProps {
  totalItemsCount: number;
  page: number;
  pageSize: number;
  onPageChange: (page: number) => void;
}

export const PaginationMenu = ({
  totalItemsCount,
  page,
  pageSize,
  onPageChange,
}: PaginationProps) => {
  return (
    <Pagination.Root
      count={totalItemsCount}
      pageSize={pageSize}
      page={page}
      onPageChange={(details: PaginationPageChangeDetails) =>
        onPageChange(details.page)
      }
    >
      <ButtonGroup variant="ghost" size="sm" wrap="wrap">
        <Pagination.PrevTrigger asChild>
          <IconButton>
            <LuChevronLeft />
          </IconButton>
        </Pagination.PrevTrigger>

        <Pagination.Items
          render={(page) => (
            <IconButton variant={{ base: "ghost", _selected: "outline" }}>
              {page.value}
            </IconButton>
          )}
        />

        <Pagination.NextTrigger asChild>
          <IconButton>
            <LuChevronRight />
          </IconButton>
        </Pagination.NextTrigger>
      </ButtonGroup>
    </Pagination.Root>
  );
};
