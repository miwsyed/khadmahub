import { ComponentType } from "react";
import { Navigate } from "react-router-dom";
import { useAuth } from "@/context/AuthContext";
import { GuardedComponentProps } from "./types/hoc.types";

/**
 * Inverse of withAuthGuard: a signed-in user who lands on /login or
 * /sign-up is redirected straight to the dashboard instead of seeing
 * the form again.
 */
export function withGuestGuard<P extends object>(Component: ComponentType<P>) {
  function Guarded(props: GuardedComponentProps<P>) {
    const { isAuthenticated, isInitializing } = useAuth();

    if (isInitializing) {
      return null;
    }

    if (isAuthenticated) {
      return <Navigate to="/home" replace />;
    }

    return <Component {...props} />;
  }

  Guarded.displayName = `withGuestGuard(${Component.displayName || Component.name || "Component"})`;
  return Guarded;
}
