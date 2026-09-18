import { useTranslation } from "react-i18next";
import { AuthLayout } from "@/components/layout/AuthLayout/AuthLayout";
import { LoginForm } from "../../components/LoginForm/LoginForm";

export function LoginPage() {
  const { t } = useTranslation();

  return (
    <AuthLayout
      eyebrow={t("auth.login.eyebrow")}
      title={t("auth.login.title")}
      subtitle={t("auth.login.subtitle")}
      footer={t("auth.login.footer")}
    >
      <LoginForm />
    </AuthLayout>
  );
}
