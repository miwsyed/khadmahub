import { useEffect, type ReactNode } from "react";
import { useTranslation } from "react-i18next";
import { BrowserRouter } from "react-router-dom";
import { AuthProvider } from "@/context/AuthContext";
import { ToastProvider } from "@/context/ToastContext";

function DirectionSync() {
  const { i18n } = useTranslation();

  useEffect(() => {
    const isRtl = i18n.language === "ar" || i18n.language === "ku";
    document.documentElement.lang = i18n.language;
    document.documentElement.dir = isRtl ? "rtl" : "ltr";
  }, [i18n.language]);

  return null;
}

export function AppProviders({ children }: { children: ReactNode }) {
  return (
    <BrowserRouter>
      <ToastProvider>
        <AuthProvider>
          <DirectionSync />
          {children}
        </AuthProvider>
      </ToastProvider>
    </BrowserRouter>
  );
}
