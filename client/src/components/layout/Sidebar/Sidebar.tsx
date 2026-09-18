import clsx from "clsx";
import { useTranslation } from "react-i18next";
import { useToast } from "@/context/ToastContext";
import { useSidebarNavigation } from "../hooks/useSidebarNavigation";
import type { NavItem, SidebarProps } from "../types/sidebar.types";
import styles from "./Sidebar.module.css";

export function Sidebar({ isOpen, onClose }: SidebarProps) {
  const { notify } = useToast();
  const { t } = useTranslation();
  const navItems = useSidebarNavigation();

  const handleNavClick = (item: NavItem) => {
    if (item.comingSoon) {
      notify({
        variant: "info",
        title: t("sidebar.comingSoonTitle", { name: item.label }),
        description: t("sidebar.comingSoonDescription"),
      });
    }
    onClose();
  };

  return (
    <>
      {isOpen ? <div className={styles.scrim} onClick={onClose} aria-hidden="true" /> : null}
      <nav className={clsx(styles.sidebar, isOpen && styles.sidebarOpen)} aria-label="Main">
        <ul className={styles.list}>
          {navItems.map((item) => (
            <li key={item.label}>
              <button
                type="button"
                className={clsx(styles.navItem, item.active && styles.navItemActive)}
                onClick={() => handleNavClick(item)}
              >
                <span className={styles.icon} aria-hidden="true">
                  {item.icon}
                </span>
                <span>{item.label}</span>
                {item.comingSoon ? <span className={styles.badge}>{t("sidebar.soon")}</span> : null}
              </button>
            </li>
          ))}
        </ul>

        <div className={styles.helpCard}>
          <p className={styles.helpTitle}>{t("sidebar.helpTitle")}</p>
          <p className={styles.helpBody}>{t("sidebar.helpBody")}</p>
        </div>
      </nav>
    </>
  );
}
