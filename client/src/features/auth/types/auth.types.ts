export interface UserRecord {
  id: string;
  fullName: string;
  username: string;
  email?: string;
  nationalId?: string;
  passwordHash?: string;
  createdAt?: string;
}

export type PublicUser = Omit<UserRecord, "passwordHash"> & {
  email?: string;
  nationalId?: string;
};

export interface LoginCredentials {
  username: string;
  password: string;
}

export interface SignUpPayload {
  fullName: string;
  username: string;
  email: string;
  nationalId?: string;
  password: string;
}

export type AuthResult =
  | { success: true; user: PublicUser; accessToken?: string }
  | { success: false; error: string };
