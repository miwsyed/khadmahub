import { useMemo, useState } from "react";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { useTranslation } from "react-i18next";
import { Link, useNavigate } from "react-router-dom";
import { Input } from "@/components/ui/Input/Input";
import { PasswordInput } from "@/components/ui/PasswordInput/PasswordInput";
import { Button } from "@/components/ui/Button/Button";
import { useAuth } from "@/context/AuthContext";
import { useToast } from "@/context/ToastContext";
import { createLoginSchema } from "../../schemas/authSchemas";
import { LoginFormValues } from "../../types/forms.types";
import { useAuthFlow } from "../../hooks/useAuthFlow";
import styles from "./LoginForm.module.css";

export function LoginForm() {
  const { login } = useAuth();
  const { notify } = useToast();
  const { t } = useTranslation();
  const navigate = useNavigate();
  const { redirectPath } = useAuthFlow();
  const [formError, setFormError] = useState<string | null>(null);
  const loginSchema = useMemo(() => createLoginSchema(t), [t]);

  const {
    register,
    handleSubmit,
    formState: { errors, isSubmitting },
  } = useForm<LoginFormValues>({
    resolver: zodResolver(loginSchema),
    defaultValues: { username: "", password: "" },
  });

  const onSubmit = async (values: LoginFormValues) => {
    setFormError(null);
    const result = await login(values);

    if (!result.success) {
      const apiError = result.error ?? t("auth.login.errorGeneric");
      setFormError(apiError);
      notify({
        variant: "error",
        title: apiError,
      });
      return;
    }

    notify({
      variant: "success",
      title: t("auth.login.successTitle"),
      description: t("auth.login.successDescription"),
    });
    navigate(redirectPath, { replace: true });
  };

  return (
    <form className={styles.form} onSubmit={handleSubmit(onSubmit)} noValidate>
      {formError ? (
        <div className={styles.formAlert} role="alert">
          {formError}
        </div>
      ) : null}

      <Input
        label={t("auth.login.username")}
        placeholder={t("auth.login.usernamePlaceholder")}
        autoComplete="username"
        error={errors.username?.message}
        {...register("username")}
      />

      <PasswordInput
        label={t("auth.login.password")}
        placeholder={t("auth.login.passwordPlaceholder")}
        autoComplete="current-password"
        error={errors.password?.message}
        {...register("password")}
      />

      <div className={styles.rowBetween}>
        <label className={styles.checkboxRow}>
          <input type="checkbox" name="rememberMe" />
          <span>{t("auth.login.rememberMe")}</span>
        </label>
        <button type="button" className={styles.linkButton}>
          {t("auth.login.forgotPassword")}
        </button>
      </div>

      <Button type="submit" fullWidth isLoading={isSubmitting}>
        {t("auth.login.submit")}
      </Button>

      <p className={styles.switchText}>
        {t("auth.login.switchText")} <Link to="/sign-up">{t("auth.login.switchLink")}</Link>
      </p>
    </form>
  );
}
