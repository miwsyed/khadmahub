import { useTranslation } from "react-i18next";

export function useTranslationValue() {
  const { t } = useTranslation();

  function translate<T extends string | undefined>(
    key: string,
    fallback?: T,
    options?: Record<string, unknown>
  ): string {
    const resolved = t(key, {
      ...(options ?? {}),
      defaultValue: fallback ?? key,
    });

    return typeof resolved === "string" ? resolved : fallback ?? key;
  }

  return { t, translate };
}
