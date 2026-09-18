export const API_BASE_URL = import.meta.env.VITE_API_BASE_URL ?? "http://localhost:5140";
export const DEFAULT_LOCALE = "en";
export const SUPPORTED_LOCALES = ["en", "ar", "ku"] as const;
export const LANGUAGE_STORAGE_KEY = "khadmahub-language";
