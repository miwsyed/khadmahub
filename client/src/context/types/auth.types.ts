import type {
  LoginCredentials,
  PublicUser,
  SignUpPayload,
} from "@/features/auth/types/auth.types";

export interface SessionData {
  user: PublicUser;
  accessToken: string;
}

export interface AuthContextValue {
  user: PublicUser | null;
  accessToken: string | null;
  isAuthenticated: boolean;
  isInitializing: boolean;
  login: (credentials: LoginCredentials) => Promise<{ success: boolean; error?: string }>;
  signUp: (payload: SignUpPayload) => Promise<{ success: boolean; error?: string }>;
  logout: () => void;
}
