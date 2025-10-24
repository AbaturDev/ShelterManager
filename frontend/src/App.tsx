import { I18nextProvider } from "react-i18next";
import { Layout } from "./Layout";
import { Toaster } from "./components/ui/toaster";
import i18n from "./i18n/i18n";
import { BrowserRouter, Route, Routes } from "react-router-dom";
import { HomePage, LoginPage } from "./pages";
import AuthProvider from "./utils/AuthProvider";
import { NonAuthRoute } from "./utils/NonAuthRoute";
import { ProtectedRoute } from "./utils/ProtectedRoute";

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
                  <Route path="/" element={<HomePage />} />
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
