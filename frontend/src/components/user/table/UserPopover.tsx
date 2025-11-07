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
import type { UserDetails } from "../../../models/user";

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

  const user = data as UserDetails;

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
              {t("user.details.header")} – {user?.name} {user?.surname}
            </Heading>
          </Popover.Header>
          <Popover.Body>
            {isLoading ? (
              <Spinner />
            ) : (
              <Stack gap={2}>
                <Text>
                  <b>{t("user.details.email")}:</b> {user?.email}
                </Text>
                <Text>
                  <b>{t("user.details.role")}:</b>{" "}
                  <Badge color={user?.role === "Admin" ? "blue.700" : "green"}>
                    {user?.role}
                  </Badge>
                </Text>
                <Text>
                  <b>{t("user.details.mustChangePassword")}</b>{" "}
                  {user?.mustChangePassword ? t("yes") : t("no")}
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
