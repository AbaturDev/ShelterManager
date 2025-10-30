import {
  Badge,
  Box,
  Heading,
  Popover,
  Spinner,
  Stack,
  Text,
} from "@chakra-ui/react";
import { useUserByIdQuery } from "../../../hooks/useUserByIdQuery";
import { useTranslation } from "react-i18next";

interface UserPopoverProps {
  userId: string;
  isOpen: boolean;
  onClose: () => void;
  anchorEl: HTMLElement | null;
}

export const UserPopover = ({
  userId,
  isOpen,
  onClose,
  anchorEl,
}: UserPopoverProps) => {
  const { t } = useTranslation();

  const { data, isLoading, error } = useUserByIdQuery(userId);

  if (!anchorEl || error) {
    onClose();
    return;
  }

  return (
    <Popover.Root
      open={isOpen}
      onOpenChange={(details) => {
        if (!details.open) {
          onClose();
        }
      }}
    >
      <Popover.Anchor asChild>
        <Box
          position="absolute"
          top={anchorEl.offsetTop}
          left={anchorEl.offsetLeft}
          width={anchorEl.offsetWidth}
          height={anchorEl.offsetHeight}
        />
      </Popover.Anchor>
      <Popover.Positioner>
        <Popover.Content maxW="400px">
          <Popover.Header>
            <Heading size="md">
              {t("user.details.header")} – {data?.name} {data?.surname}
            </Heading>
          </Popover.Header>
          <Popover.Body>
            {isLoading ? (
              <Spinner />
            ) : (
              <Stack gap={2}>
                <Text>
                  <b>{t("user.details.email")}:</b> {data?.email}
                </Text>
                <Text>
                  <b>{t("user.details.role")}:</b>{" "}
                  <Badge color={data?.role === "Admin" ? "blue.700" : "green"}>
                    {data?.role}
                  </Badge>
                </Text>
                <Text>
                  <b>{t("user.details.mustChangePassword")}</b>{" "}
                  {data?.mustChangePassword ? t("yes") : t("no")}
                </Text>
              </Stack>
            )}
          </Popover.Body>
          <Popover.CloseTrigger onClick={onClose} />
        </Popover.Content>
      </Popover.Positioner>
    </Popover.Root>
  );
};
