import { useLocation } from "react-router-dom";
import type { LoginFormLocationState } from "../components/LoginForm/types/loginForm.types";

export function useAuthFlow() {
  const location = useLocation();

  const redirectPath =
    (location.state as LoginFormLocationState | null)?.from?.pathname ?? "/home";

  return { redirectPath };
}
