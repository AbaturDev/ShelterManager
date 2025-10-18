import { I18nextProvider } from "react-i18next";
import { Layout } from "./Layout";
import { Toaster } from "./components/ui/toaster";
import i18n from "./i18n/i18n";
import { BrowserRouter, Route, Routes } from "react-router-dom";
import { Home } from "./pages";

function App() {
  return (
    <>
      <Toaster />
      <I18nextProvider i18n={i18n}>
        <BrowserRouter>
          <Routes>
            <Route element={<Layout />}>
              <Route path="/" element={<Home />} />
            </Route>
          </Routes>
        </BrowserRouter>
      </I18nextProvider>
    </>
  );
}

export default App;
