import { initReactI18next } from "react-i18next";
import i18nBackend from "i18next-http-backend";
import i18n from "i18next";
import en from "./languages/en.json";
import pl from "./languages/pl.json";

export type I18nLocale = typeof en | typeof pl;

i18n
  .use(i18nBackend)
  .use(initReactI18next)
  .init({
    resources: {
      en: {
        translation: en,
      },
      pl: {
        translation: pl,
      },
    },
    fallbackLng: localStorage.getItem("lang") ?? "en",

    interpolation: {
      escapeValue: false,
    },
  });

export default i18n;
