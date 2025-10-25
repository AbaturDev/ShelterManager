import { I18nextProvider } from "react-i18next";
import { Layout } from "./Layout";
import { Toaster } from "./components/ui/toaster";
import i18n from "./i18n/i18n";
import { BrowserRouter, Route, Routes } from "react-router-dom";
import { ChangePasswordPage, HomePage, LoginPage } from "./pages";
import AuthProvider from "./utils/AuthProvider";
import { NonAuthRoute } from "./utils/NonAuthRoute";
import { ProtectedRoute } from "./utils/ProtectedRoute";
import { MustChangePasswordRoute } from "./utils/MustChangePasswordRoute";

function App() {
  return (
    <>
      <Toaster />
      <I18nextProvider i18n={i18n}>
        <BrowserRouter>
          <AuthProvider>
            <Routes>
              <Route element={<NonAuthRoute />}>
                <Route path="/login" element={<LoginPage />} />
              </Route>
              <Route element={<ProtectedRoute />}>
                <Route element={<Layout />}>
                  <Route
                    path="/change-password"
                    element={<ChangePasswordPage />}
                  />
                  <Route element={<MustChangePasswordRoute />}>
                    <Route path="/" element={<HomePage />} />
                  </Route>
                </Route>
              </Route>
            </Routes>
          </AuthProvider>
        </BrowserRouter>
      </I18nextProvider>
    </>
  );
}

export default App;
