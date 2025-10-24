import { Navigate, Outlet } from "react-router-dom";
import { useAuth } from "./AuthProvider";

export const NonAuthRoute = () => {
  const auth = useAuth();

  if (auth?.jwtToken) {
    return <Navigate to="/" />;
  }

  return <Outlet />;
};
