export interface SignUpFormPayloadResult {
  payload: {
    fullName: string;
    username: string;
    email: string;
    nationalId?: string;
    password: string;
  };
  errorMessage: string | undefined;
}
