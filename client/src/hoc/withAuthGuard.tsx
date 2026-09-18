import { ComponentType } from "react";
import { Navigate, useLocation } from "react-router-dom";
import { useAuth } from "@/context/AuthContext";
import { GuardedComponentProps } from "./types/hoc.types";

/**
 * Wraps a page component so it can only render for a signed-in user.
 * Anonymous visitors are bounced to /login with the page they wanted
 * preserved in location state, so login can send them back afterwards.
 */
export function withAuthGuard<P extends object>(Component: ComponentType<P>) {
  function Guarded(props: GuardedComponentProps<P>) {
    const { isAuthenticated, isInitializing } = useAuth();
    const location = useLocation();

    if (isInitializing) {
      return null;
    }

    if (!isAuthenticated) {
      return <Navigate to="/login" replace state={{ from: location }} />;
    }

    return <Component {...props} />;
  }

  Guarded.displayName = `withAuthGuard(${Component.displayName || Component.name || "Component"})`;
  return Guarded;
}
