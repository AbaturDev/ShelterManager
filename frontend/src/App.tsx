import { I18nextProvider } from "react-i18next";
import { Toaster } from "./components/ui/toaster";
import i18n from "./i18n/i18n";
import { BrowserRouter } from "react-router-dom";
import { AppRoutes } from "./AppRoutes";
import AuthProvider from "./utils/AuthProvider";

function App() {
  return (
    <>
      <Toaster />
      <I18nextProvider i18n={i18n}>
        <BrowserRouter>
          <AuthProvider>
            <AppRoutes />
          </AuthProvider>
        </BrowserRouter>
      </I18nextProvider>
    </>
  );
}

export default App;
