import { useTranslation } from "react-i18next";
import { AuthLayout } from "@/components/layout/AuthLayout/AuthLayout";
import { SignUpForm } from "../../components/SignUpForm/SignUpForm";

export function SignUpPage() {
  const { t } = useTranslation();

  return (
    <AuthLayout
      eyebrow={t("auth.signup.eyebrow")}
      title={t("auth.signup.title")}
      subtitle={t("auth.signup.subtitle")}
    >
      <SignUpForm />
    </AuthLayout>
  );
}
