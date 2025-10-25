import { Box, Icon, Menu, Portal } from "@chakra-ui/react";
import { useTranslation } from "react-i18next";
import { FaUserCircle } from "react-icons/fa";
import { MdLogout, MdKey } from "react-icons/md";
import { ImProfile } from "react-icons/im";
import { useAuth } from "../../utils/AuthProvider";
import { useNavigate } from "react-router-dom";

export const ProfileMenu = () => {
  const auth = useAuth();
  const { t } = useTranslation();
  const navigate = useNavigate();

  return (
    <Menu.Root positioning={{ placement: "bottom" }}>
      <Menu.Trigger rounded="full" focusRing="outside">
        <Icon as={FaUserCircle} boxSize={"8"} />
      </Menu.Trigger>
      <Portal>
        <Menu.Positioner>
          <Menu.Content>
            <Menu.Item value="profile">
              <Icon as={ImProfile} />
              <Box flex={"1"}>{t("profileMenu.profile")}</Box>
            </Menu.Item>
            <Menu.Item
              value="password"
              onSelect={() => navigate("/change-password")}
            >
              <Icon as={MdKey} />
              <Box flex={"1"}>{t("profileMenu.changePassword")}</Box>
            </Menu.Item>
            <Menu.Separator />
            <Menu.Item value="logout" onSelect={auth?.logout}>
              <Icon as={MdLogout} />
              <Box flex={"1"}>{t("profileMenu.logout")}</Box>
            </Menu.Item>
          </Menu.Content>
        </Menu.Positioner>
      </Portal>
    </Menu.Root>
  );
};
