import { createPortal } from "react-dom";
import { Toast } from "./Toast";
import type { ToastViewportProps } from "./types/toast.types";
import styles from "./Toast.module.css";

export function ToastViewport({ toasts, onDismiss }: ToastViewportProps) {
  if (toasts.length === 0) return null;

  return createPortal(
    <div className={styles.viewport} aria-live="polite" aria-atomic="false">
      {toasts.map((toast) => (
        <Toast key={toast.id} toast={toast} onDismiss={onDismiss} />
      ))}
    </div>,
    document.body
  );
}
