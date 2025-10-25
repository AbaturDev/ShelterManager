import { Navigate, Outlet } from "react-router-dom";
import { useAuth } from "./AuthProvider";

export const MustChangePasswordRoute = () => {
  const auth = useAuth();

  if (auth?.mustChangePassword) {
    return <Navigate to="/change-password" />;
  }

  return <Outlet />;
};
