import clsx from "clsx";
import { useTranslation } from "react-i18next";
import { LANGUAGES } from "../constants/language.constants";
import styles from "./LanguageSwitcher.module.css";

export function LanguageSwitcher() {
  const { i18n, t } = useTranslation();

  const handleChange = (language: (typeof LANGUAGES)[number]["code"]) => {
    void i18n.changeLanguage(language);
    localStorage.setItem("khadmahub-language", language);
    document.documentElement.lang = language;
    document.documentElement.dir = language === "ar" || language === "ku" ? "rtl" : "ltr";
  };

  return (
    <div className={styles.switcher} aria-label={t("common.languageLabel")}>
      {LANGUAGES.map((language) => (
        <button
          key={language.code}
          type="button"
          className={clsx(styles.option, i18n.language === language.code && styles.active)}
          onClick={() => handleChange(language.code)}
          aria-pressed={i18n.language === language.code}
        >
          {language.label}
        </button>
      ))}
    </div>
  );
}
