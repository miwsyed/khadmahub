import { useTranslation } from "react-i18next";
import type { NavItem } from "../types/sidebar.types";

export function useSidebarNavigation(): NavItem[] {
  const { t } = useTranslation();

  return [
    { label: t("sidebar.dashboard"), icon: "▣", active: true },
    { label: t("sidebar.myApplications"), icon: "▤", comingSoon: true },
    { label: t("sidebar.appointments"), icon: "◷", comingSoon: true },
    { label: t("sidebar.payments"), icon: "◈", comingSoon: true },
    { label: t("sidebar.directorates"), icon: "▦", comingSoon: true },
    { label: t("sidebar.settings"), icon: "⚙", comingSoon: true },
  ];
}
