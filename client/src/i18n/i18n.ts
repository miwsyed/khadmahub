import i18next from "i18next";
import { initReactI18next } from "react-i18next";
import { resources } from "./resources";

const LANGUAGE_KEY = "khadmahub-language";
const browserLanguage = navigator.language.toLowerCase();
const detectedLanguage =
  browserLanguage.startsWith("ar") ? "ar" : browserLanguage.startsWith("ku") ? "ku" : "en";
const savedLanguage = localStorage.getItem(LANGUAGE_KEY);

const initialLanguage = savedLanguage ?? detectedLanguage;

void i18next.use(initReactI18next).init({
  resources,
  lng: initialLanguage,
  fallbackLng: "en",
  supportedLngs: ["en", "ar", "ku"],
  interpolation: {
    escapeValue: false,
  },
  react: {
    useSuspense: false,
  },
});

export default i18next;
