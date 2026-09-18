import { StrictMode } from "react";
import { createRoot } from "react-dom/client";
import "./i18n/i18n";
import "./styles/globals.css";
import App from "./App";

const preferredLanguage =
  localStorage.getItem("khadmahub-language") ??
  (navigator.language.toLowerCase().startsWith("ar")
    ? "ar"
    : navigator.language.toLowerCase().startsWith("ku")
      ? "ku"
      : "en");

document.documentElement.lang = preferredLanguage;
document.documentElement.dir = preferredLanguage === "ar" || preferredLanguage === "ku" ? "rtl" : "ltr";

const container = document.getElementById("root");

if (!container) {
  throw new Error("Root element (#root) was not found in index.html");
}

createRoot(container).render(
  <StrictMode>
    <App />
  </StrictMode>
);
