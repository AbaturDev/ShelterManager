import { Navigate, Outlet } from "react-router-dom";
import { useAuth } from "./AuthProvider";
import { toaster } from "../components/ui/toaster";
import { useTranslation } from "react-i18next";
import { useEffect } from "react";

interface RoleBasedRouteProps {
  roles: string[];
}

export const RoleBasedRoute = ({ roles }: RoleBasedRouteProps) => {
  const { t } = useTranslation();
  const auth = useAuth();

  const hasAccess = auth?.role ? roles.includes(auth.role) : false;

  useEffect(() => {
    if (auth?.role && !hasAccess) {
      queueMicrotask(() => {
        toaster.create({
          type: "error",
          title: t("route.roleBased.errorToast.title"),
          description: t("route.roleBased.errorToast.description"),
          closable: true,
        });
      });
    }
  }, [auth?.role, hasAccess, t]);

  if (!auth?.role) {
    return null;
  }

  if (!hasAccess) {
    return <Navigate to="/" replace />;
  }

  return <Outlet />;
};
