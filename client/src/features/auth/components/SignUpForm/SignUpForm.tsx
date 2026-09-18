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
import { createSignUpSchema } from "../../schemas/authSchemas";
import { SignUpFormValues } from "../../types/forms.types";
import { useSignUpFlow } from "../../hooks/useSignUpFlow";
import styles from "./SignUpForm.module.css";

export function SignUpForm() {
  const { signUp } = useAuth();
  const { notify } = useToast();
  const { t } = useTranslation();
  const navigate = useNavigate();
  const { preparePayload, getErrorMessage } = useSignUpFlow();
  const [formError, setFormError] = useState<string | null>(null);
  const signUpSchema = useMemo(() => createSignUpSchema(t), [t]);

  const {
    register,
    handleSubmit,
    formState: { errors, isSubmitting },
  } = useForm<SignUpFormValues>({
    resolver: zodResolver(signUpSchema),
    defaultValues: {
      fullName: "",
      username: "",
      email: "",
      nationalId: "",
      password: "",
      confirmPassword: "",
    },
  });

  const onSubmit = async (values: SignUpFormValues) => {
    setFormError(null);
    const payload = preparePayload(values);
    const result = await signUp(payload);

    if (!result.success) {
      const apiError = getErrorMessage(result.error, t("auth.signup.errorGeneric"));
      setFormError(apiError);
      notify({ variant: "error", title: apiError });
      return;
    }

    notify({
      variant: "success",
      title: t("auth.signup.successTitle"),
      description: t("auth.signup.successDescription"),
    });
    navigate("/login", { replace: true });
  };

  return (
    <form className={styles.form} onSubmit={handleSubmit(onSubmit)} noValidate>
      {formError ? (
        <div className={styles.formAlert} role="alert">
          {formError}
        </div>
      ) : null}

      <Input
        label={t("auth.signup.fullName")}
        placeholder={t("auth.signup.fullNamePlaceholder")}
        autoComplete="name"
        error={errors.fullName?.message}
        {...register("fullName")}
      />

      <div className={styles.grid2}>
        <Input
          label={t("auth.signup.username")}
          placeholder={t("auth.signup.usernamePlaceholder")}
          autoComplete="username"
          error={errors.username?.message}
          {...register("username")}
        />
        <Input
          label={`${t("auth.signup.nationalId")} (${t("common.optional")})`}
          placeholder={t("auth.signup.nationalIdPlaceholder")}
          inputMode="numeric"
          error={errors.nationalId?.message}
          {...register("nationalId")}
        />
      </div>

      <Input
        label={t("auth.signup.email")}
        type="email"
        placeholder={t("auth.signup.emailPlaceholder")}
        autoComplete="email"
        error={errors.email?.message}
        {...register("email")}
      />

      <div className={styles.grid2}>
        <PasswordInput
          label={t("auth.signup.password")}
          placeholder={t("auth.signup.passwordPlaceholder")}
          autoComplete="new-password"
          error={errors.password?.message}
          {...register("password")}
        />
        <PasswordInput
          label={t("auth.signup.confirmPassword")}
          placeholder={t("auth.signup.confirmPasswordPlaceholder")}
          autoComplete="new-password"
          error={errors.confirmPassword?.message}
          {...register("confirmPassword")}
        />
      </div>

      <Button type="submit" fullWidth isLoading={isSubmitting}>
        {t("auth.signup.submit")}
      </Button>

      <p className={styles.switchText}>
        {t("auth.signup.switchText")} <Link to="/login">{t("auth.signup.switchLink")}</Link>
      </p>
    </form>
  );
}
