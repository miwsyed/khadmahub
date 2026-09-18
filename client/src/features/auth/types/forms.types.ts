export interface LoginFormValues {
  username: string;
  password: string;
}

export interface SignUpFormValues {
  fullName: string;
  username: string;
  email: string;
  nationalId?: string;
  password: string;
  confirmPassword: string;
}
