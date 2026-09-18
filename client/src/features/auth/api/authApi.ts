import { API_BASE_URL, DEFAULT_LOCALE, LANGUAGE_STORAGE_KEY } from "../constants/auth.constants";
import { AuthResult, LoginCredentials, PublicUser, SignUpPayload } from "../types/auth.types";
import { ProblemDetailsPayload } from "../types/api.types";

async function parseApiError(response: Response): Promise<string> {
  try {
    const payload = (await response.json()) as ProblemDetailsPayload;
    if (payload.detail) {
      return payload.detail;
    }
    if (payload.errors) {
      const firstError = Object.values(payload.errors)[0]?.[0];
      if (firstError) {
        return firstError;
      }
    }
    if (payload.title) {
      return payload.title;
    }
  } catch {
    // Ignore JSON parsing failures and fall back to status text below.
  }

  return response.statusText || "The request could not be completed.";
}

function getCurrentLocale(): string {
  const savedLanguage = localStorage.getItem(LANGUAGE_STORAGE_KEY);
  if (savedLanguage === "ar" || savedLanguage === "ku" || savedLanguage === "en") {
    return savedLanguage;
  }

  const browserLanguage = navigator.language.toLowerCase();
  if (browserLanguage.startsWith("ar")) {
    return "ar";
  }
  if (browserLanguage.startsWith("ku")) {
    return "ku";
  }

  return DEFAULT_LOCALE;
}

async function requestJson<T>(url: string, options: RequestInit): Promise<T> {
  const locale = getCurrentLocale();
  const response = await fetch(url, {
    headers: {
      "Content-Type": "application/json",
      "Accept-Language": locale,
      "X-Locale": locale,
      ...(options.headers ?? {}),
    },
    credentials: "include",
    ...options,
  });

  if (!response.ok) {
    throw new Error(await parseApiError(response));
  }

  return (await response.json()) as T;
}

export const authApi = {
  async login({ username, password }: LoginCredentials): Promise<AuthResult> {
    try {
      const response = await requestJson<{
        accessToken: string;
        expiresAt: string;
        user: PublicUser;
      }>(`${API_BASE_URL}/api/v1/auth/login`, {
        method: "POST",
        body: JSON.stringify({ username, password }),
      });

      return {
        success: true,
        user: response.user,
        accessToken: response.accessToken,
      };
    } catch (error) {
      return {
        success: false,
        error: error instanceof Error ? error.message : "Login failed.",
      };
    }
  },

  async signUp(payload: SignUpPayload): Promise<AuthResult> {
    try {
      const response = await requestJson<{
        user: PublicUser;
        accessToken?: string;
        expiresAt?: string;
      }>(`${API_BASE_URL}/api/v1/auth/sign-up`, {
        method: "POST",
        body: JSON.stringify({
          fullName: payload.fullName,
          username: payload.username,
          email: payload.email,
          nationalId: payload.nationalId || undefined,
          password: payload.password,
        }),
      });

      return {
        success: true,
        user: response.user,
        accessToken: response.accessToken,
      };
    } catch (error) {
      return {
        success: false,
        error: error instanceof Error ? error.message : "Sign-up failed.",
      };
    }
  },
};
