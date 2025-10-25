import { Box, HStack } from "@chakra-ui/react";
import { PaginationMenu } from "./PaginationMenu";
import { PageSizeSelector } from "./PageSizeSelector";

interface PaginationFooterProps {
  page: number;
  pageSize: number;
  totalItemsCount: number;
  onPageChange: (page: number) => void;
  onPageSizeChange: (pageSize: number) => void;
}

export const PaginationFooter = ({
  page,
  pageSize,
  totalItemsCount,
  onPageChange,
  onPageSizeChange,
}: PaginationFooterProps) => {
  return (
    <HStack justify="space-between" align="center" width="full">
      <Box flex="1" />

      <Box flex="1" display="flex" justifyContent="center">
        <PaginationMenu
          page={page}
          pageSize={pageSize}
          totalItemsCount={totalItemsCount}
          onPageChange={onPageChange}
        />
      </Box>

      <Box flex="1" display="flex" justifyContent="flex-end">
        <PageSizeSelector
          pageSize={pageSize}
          onPageSizeChange={onPageSizeChange}
        />
      </Box>
    </HStack>
  );
};
