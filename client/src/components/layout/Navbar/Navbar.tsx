import { useState } from "react";
import { useTranslation } from "react-i18next";
import { SealMark } from "@/components/ui/EmblemPattern/SealMark";
import { LanguageSwitcher } from "@/components/ui/LanguageSwitcher/LanguageSwitcher";
import { useAuth } from "@/context/AuthContext";
import { useToast } from "@/context/ToastContext";
import { useNavigate } from "react-router-dom";
import { getInitials } from "../utils/userInitials";
import type { NavbarProps } from "../types/navbar.types";
import styles from "./Navbar.module.css";

export function Navbar({ onToggleSidebar }: NavbarProps) {
  const { user, logout } = useAuth();
  const { notify } = useToast();
  const { t } = useTranslation();
  const navigate = useNavigate();
  const [menuOpen, setMenuOpen] = useState(false);

  const handleLogout = () => {
    logout();
    notify({
      variant: "info",
      title: t("common.signOut"),
      description: t("common.comeBackSoon"),
    });
    navigate("/login", { replace: true });
  };

  return (
    <header className={styles.navbar}>
      <div className={styles.left}>
        <button
          type="button"
          className={styles.menuButton}
          onClick={onToggleSidebar}
          aria-label={t("common.toggleNavigationMenu")}
        >
          <span />
          <span />
          <span />
        </button>
        <div className={styles.brand}>
          <div className={styles.brandLabel}>
            <SealMark size={30} tone="dark" />
            <span>{t("common.brandName")}</span>
          </div>
          <LanguageSwitcher />
        </div>
      </div>

      <div className={styles.right}>
        <span className={styles.governorate}>{t("common.government")}</span>
        <div className={styles.userMenu}>
          <button
            type="button"
            className={styles.userTrigger}
            onClick={() => setMenuOpen((open) => !open)}
            aria-haspopup="menu"
            aria-expanded={menuOpen}
          >
            <span className={styles.avatar}>{getInitials(user?.fullName ?? "?")}</span>
            <span className={styles.userName}>{user?.fullName}</span>
          </button>

          {menuOpen ? (
            <div className={styles.dropdown} role="menu">
              <div className={styles.dropdownMeta}>
                <p className={styles.dropdownName}>{user?.fullName}</p>
                <p className={styles.dropdownEmail}>{user?.email}</p>
              </div>
              <button
                type="button"
                className={styles.logoutButton}
                onClick={handleLogout}
                role="menuitem"
              >
                {t("common.signOut")}
              </button>
            </div>
          ) : null}
        </div>
      </div>
    </header>
  );
}
