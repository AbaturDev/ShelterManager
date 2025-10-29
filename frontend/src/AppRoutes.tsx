import { Route, Routes } from "react-router-dom";
import { AuthLayout, PublicLayout } from "./Layout";
import {
  AnimalsPage,
  ChangePasswordPage,
  ForgotPasswordPage,
  HomePage,
  LoginPage,
  ResetPasswordPage,
  SpeciesDetailsPage,
  SpeciesPage,
  UsersPage,
} from "./pages";
import {
  NonAuthRoute,
  ProtectedRoute,
  MustChangePasswordRoute,
  RoleBasedRoute,
} from "./utils";

export const AppRoutes = () => {
  return (
    <Routes>
      <Route element={<PublicLayout />}>
        <Route element={<NonAuthRoute />}>
          <Route path="/login" element={<LoginPage />} />
          <Route path="/forgot-password" element={<ForgotPasswordPage />} />
          <Route path="/reset-password" element={<ResetPasswordPage />} />
        </Route>
      </Route>
      <Route element={<ProtectedRoute />}>
        <Route element={<AuthLayout />}>
          <Route path="/change-password" element={<ChangePasswordPage />} />
          <Route element={<MustChangePasswordRoute />}>
            <Route path="/" element={<HomePage />} />
            <Route path="/species" element={<SpeciesPage />} />
            <Route path="/species/:id" element={<SpeciesDetailsPage />} />
            <Route path="/animals" element={<AnimalsPage />} />
            <Route element={<RoleBasedRoute roles={["Admin"]} />}>
              <Route path="/users" element={<UsersPage />} />
            </Route>
          </Route>
        </Route>
      </Route>
    </Routes>
  );
};
