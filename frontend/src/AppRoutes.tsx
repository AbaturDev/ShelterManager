import { Route, Routes } from "react-router-dom";
import { AuthLayout, PublicLayout } from "./Layout";
import { NonAuthRoute } from "./utils/NonAuthRoute";
import { ProtectedRoute } from "./utils/ProtectedRoute";
import {
  ChangePasswordPage,
  ForgotPasswordPage,
  HomePage,
  LoginPage,
  ResetPasswordPage,
  SpeciesDetailsPage,
  SpeciesPage,
} from "./pages";
import { MustChangePasswordRoute } from "./utils/MustChangePasswordRoute";

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
          </Route>
        </Route>
      </Route>
    </Routes>
  );
};
