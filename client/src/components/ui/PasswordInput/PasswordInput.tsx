import { forwardRef, useId, useState } from "react";
import clsx from "clsx";
import { useTranslation } from "react-i18next";
import inputStyles from "@/components/ui/Input/Input.module.css";
import type { PasswordInputProps } from "../types/passwordInput.types";
import styles from "./PasswordInput.module.css";

export const PasswordInput = forwardRef<HTMLInputElement, PasswordInputProps>(
  ({ label, error, id, className, ...rest }, ref) => {
    const [visible, setVisible] = useState(false);
    const { t } = useTranslation();
    const generatedId = useId();
    const inputId = id ?? generatedId;
    const errorId = `${inputId}-error`;

    return (
      <div className={clsx(inputStyles.field, className)}>
        <label htmlFor={inputId} className={inputStyles.label}>
          {label}
        </label>
        <div className={styles.wrapper}>
          <input
            ref={ref}
            id={inputId}
            type={visible ? "text" : "password"}
            className={clsx(inputStyles.input, styles.input, error && inputStyles.inputError)}
            aria-invalid={Boolean(error)}
            aria-describedby={error ? errorId : undefined}
            {...rest}
          />
          <button
            type="button"
            className={styles.toggle}
            onClick={() => setVisible((current) => !current)}
            aria-label={visible ? t("common.hidePassword") : t("common.showPassword")}
            aria-pressed={visible}
          >
            {visible ? t("common.hide") : t("common.show")}
          </button>
        </div>
        {error ? (
          <p id={errorId} className={inputStyles.errorText}>
            {error}
          </p>
        ) : null}
      </div>
    );
  }
);

PasswordInput.displayName = "PasswordInput";
