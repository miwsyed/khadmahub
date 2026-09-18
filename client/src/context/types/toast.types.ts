export type ToastVariant = "success" | "error" | "info";

export interface ToastItem {
  id: string;
  variant: ToastVariant;
  title: string;
  description?: string;
}

export interface ToastContextValue {
  notify: (toast: Omit<ToastItem, "id">) => void;
  dismiss: (id: string) => void;
}
