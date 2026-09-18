import type { ToastItem } from "@/context/ToastContext";

export interface ToastProps {
  toast: ToastItem;
  onDismiss: (id: string) => void;
}

export interface ToastViewportProps {
  toasts: ToastItem[];
  onDismiss: (id: string) => void;
}
