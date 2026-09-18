import { useTranslation } from "react-i18next";
import { SealMark } from "@/components/ui/EmblemPattern/SealMark";
import { LatticePattern } from "@/components/ui/EmblemPattern/LatticePattern";
import { LanguageSwitcher } from "@/components/ui/LanguageSwitcher/LanguageSwitcher";
import { useAuthLayoutDirectorates } from "../hooks/useAuthLayoutDirectorates";
import type { AuthLayoutProps } from "../types/authLayout.types";
import styles from "./AuthLayout.module.css";

export function AuthLayout({ eyebrow, title, subtitle, children, footer }: AuthLayoutProps) {
  const { t } = useTranslation();
  const directorates = useAuthLayoutDirectorates();

  return (
    <div className={styles.shell}>
      <aside className={styles.brandPanel}>
        <LatticePattern />
        <div className={styles.brandContent}>
          <div className={styles.brandMark}>
            <div className={styles.brandTitle}>
              <SealMark size={52} tone="light" />
              <span className={styles.brandName}>{t("common.brandName")}</span>
            </div>
            <LanguageSwitcher />
          </div>

          <div className={styles.brandCopy}>
            <p className={styles.brandKicker}>{t("common.government")}</p>
            <h1 className={styles.brandHeadline}>{t("auth.brand.headline")}</h1>
            <p className={styles.brandBody}>{t("auth.brand.body")}</p>
          </div>

          <ul className={styles.directorateList}>
            {directorates.map((name) => (
              <li key={name}>{name}</li>
            ))}
          </ul>
        </div>
      </aside>

      <main className={styles.formPanel}>
        <div className={styles.formPanelInner}>
          <div className={styles.mobileBrand}>
            <div className={styles.mobileBrandLabel}>
              <SealMark size={36} tone="dark" />
              <span>{t("common.brandName")}</span>
            </div>
            <LanguageSwitcher />
          </div>

          <div className={styles.formCard}>
            <p className={styles.eyebrow}>{eyebrow}</p>
            <h2 className={styles.title}>{title}</h2>
            <p className={styles.subtitle}>{subtitle}</p>
            {children}
          </div>

          {footer ? <div className={styles.footer}>{footer}</div> : null}
        </div>
      </main>
    </div>
  );
}
