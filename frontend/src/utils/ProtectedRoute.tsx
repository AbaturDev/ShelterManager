import { Navigate, Outlet } from "react-router-dom";
import { useAuth } from "./AuthProvider";

export const ProtectedRoute = () => {
  const auth = useAuth();

  if (!auth?.jwtToken) {
    return <Navigate to="/login" />;
  }

  return <Outlet />;
};
