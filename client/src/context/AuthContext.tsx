import {
  createContext,
  useCallback,
  useContext,
  useMemo,
  useState,
  ReactNode,
} from "react";
import { storage } from "@/lib/storage";
import { authApi } from "@/features/auth/api/authApi";
import type {
  LoginCredentials,
  SignUpPayload,
} from "@/features/auth/types/auth.types";
import { SESSION_KEY } from "./constants/auth.constants";
import type { SessionData, AuthContextValue } from "./types/auth.types";

const AuthContext = createContext<AuthContextValue | undefined>(undefined);

export function AuthProvider({ children }: { children: ReactNode }) {
  const [session, setSession] = useState<SessionData | null>(() =>
    storage.get<SessionData>(SESSION_KEY)
  );
  const user = session?.user ?? null;
  const [accessToken, setAccessToken] = useState<string | null>(() => session?.accessToken ?? null);
  // Session is read synchronously from storage above; kept for symmetry with
  // a real app where session restoration would involve a token round-trip.
  const [isInitializing] = useState(false);

  const login = useCallback(async (credentials: LoginCredentials) => {
    const result = await authApi.login(credentials);
    if (!result.success) {
      return { success: false, error: result.error };
    }

    const nextSession: SessionData = {
      user: result.user,
      accessToken: result.accessToken ?? "",
    };

    setSession(nextSession);
    setAccessToken(nextSession.accessToken);
    storage.set(SESSION_KEY, nextSession);
    return { success: true };
  }, []);

  const signUp = useCallback(async (payload: SignUpPayload) => {
    const result = await authApi.signUp(payload);
    if (!result.success) {
      return { success: false, error: result.error };
    }

    return { success: true };
  }, []);

  const logout = useCallback(() => {
    setSession(null);
    setAccessToken(null);
    storage.remove(SESSION_KEY);
  }, []);

  const value = useMemo<AuthContextValue>(
    () => ({
      user,
      accessToken,
      isAuthenticated: user !== null,
      isInitializing,
      login,
      signUp,
      logout,
    }),
    [user, accessToken, isInitializing, login, signUp, logout]
  );

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
}

export function useAuth(): AuthContextValue {
  const context = useContext(AuthContext);
  if (!context) {
    throw new Error("useAuth must be used within an AuthProvider");
  }
  return context;
}
