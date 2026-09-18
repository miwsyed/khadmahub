import { z } from "zod";

export function createLoginSchema(t: (key: string) => string) {
  return z.object({
    username: z.string().min(1, t("auth.validation.usernameRequired")),
    password: z.string().min(1, t("auth.validation.passwordRequired")),
  });
}

export function createSignUpSchema(t: (key: string) => string) {
  return z
    .object({
      fullName: z
        .string()
        .trim()
        .min(3, t("auth.validation.fullNameRequired")),
      username: z
        .string()
        .trim()
        .min(4, t("auth.validation.usernameMin"))
        .regex(/^[a-zA-Z0-9._]+$/, t("auth.validation.usernamePattern")),
      email: z.string().trim().email(t("auth.validation.emailInvalid")),
      nationalId: z
        .string()
        .trim()
        .refine(
          (value) => value === "" || /^\d{8,12}$/.test(value),
          t("auth.validation.nationalIdPattern")
        ),
      password: z
        .string()
        .min(8, t("auth.validation.passwordMin"))
        .regex(/[A-Z]/, t("auth.validation.passwordUppercase"))
        .regex(/[0-9]/, t("auth.validation.passwordNumber")),
      confirmPassword: z.string(),
    })
    .refine((values) => values.password === values.confirmPassword, {
      message: t("auth.validation.confirmPasswordMatch"),
      path: ["confirmPassword"],
    });
}
