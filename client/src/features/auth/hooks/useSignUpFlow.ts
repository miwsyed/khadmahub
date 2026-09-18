import type { SignUpPayload } from "../types/auth.types";
import type { SignUpFormValues } from "../types/forms.types";

export function useSignUpFlow() {
  const preparePayload = (values: SignUpFormValues): SignUpPayload => ({
    fullName: values.fullName,
    username: values.username,
    email: values.email,
    nationalId: values.nationalId?.trim() || undefined,
    password: values.password,
  });

  const getErrorMessage = (error: string | undefined, fallback: string): string =>
    error ?? fallback;

  return { preparePayload, getErrorMessage };
}
