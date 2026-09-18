import { Navigate, Route, Routes } from "react-router-dom";
import { withAuthGuard } from "@/hoc/withAuthGuard";
import { withGuestGuard } from "@/hoc/withGuestGuard";
import { LoginPage } from "@/features/auth/pages/LoginPage/LoginPage";
import { SignUpPage } from "@/features/auth/pages/SignUpPage/SignUpPage";
import { HomePage } from "@/features/dashboard/pages/HomePage/HomePage";

const GuardedLoginPage = withGuestGuard(LoginPage);
const GuardedSignUpPage = withGuestGuard(SignUpPage);
const GuardedHomePage = withAuthGuard(HomePage);

export function AppRoutes() {
  return (
    <Routes>
      <Route path="/" element={<Navigate to="/login" replace />} />
      <Route path="/login" element={<GuardedLoginPage />} />
      <Route path="/sign-up" element={<GuardedSignUpPage />} />
      <Route path="/home" element={<GuardedHomePage />} />
      <Route path="*" element={<Navigate to="/login" replace />} />
    </Routes>
  );
}
