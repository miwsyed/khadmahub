import clsx from "clsx";
import type { ToastVariant } from "@/context/types/toast.types";
import type { ToastProps } from "./types/toast.types";
import styles from "./Toast.module.css";

const ICONS: Record<ToastVariant, string> = {
  success: "✓",
  error: "!",
  info: "i",
};

export function Toast({ toast, onDismiss }: ToastProps) {
  return (
    <div
      className={clsx(styles.toast, styles[toast.variant])}
      role={toast.variant === "error" ? "alert" : "status"}
    >
      <span className={styles.icon} aria-hidden="true">
        {ICONS[toast.variant]}
      </span>
      <div className={styles.body}>
        <p className={styles.title}>{toast.title}</p>
        {toast.description ? (
          <p className={styles.description}>{toast.description}</p>
        ) : null}
      </div>
      <button
        type="button"
        className={styles.closeButton}
        aria-label="Dismiss notification"
        onClick={() => onDismiss(toast.id)}
      >
        ×
      </button>
    </div>
  );
}
