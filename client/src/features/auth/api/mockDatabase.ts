import { storage } from "@/lib/storage";
import type { UserRecord } from "../types/auth.types";

/**
 * Stand-in for a real "Users" table.
 *
 * In production this file is deleted and `authApi.ts` calls a real backend
 * (e.g. POST /api/auth/login) that queries the government identity
 * database. Until that endpoint exists, records live in localStorage so the
 * rest of the app — forms, guards, layouts — can be built and demoed
 * against something that behaves like a real credentials store.
 */

const USERS_KEY = "khadmahub.db.users";

/** SHA-256 the password client-side before it ever touches storage.
 *  This is NOT a substitute for server-side salting/hashing (bcrypt/argon2) —
 *  it only prevents plaintext passwords from sitting in localStorage during
 *  this demo phase. */
export async function hashPassword(password: string): Promise<string> {
  const encoded = new TextEncoder().encode(password);
  const digest = await window.crypto.subtle.digest("SHA-256", encoded);
  return Array.from(new Uint8Array(digest))
    .map((byte) => byte.toString(16).padStart(2, "0"))
    .join("");
}

function readTable(): UserRecord[] {
  return storage.get<UserRecord[]>(USERS_KEY) ?? [];
}

function writeTable(users: UserRecord[]): void {
  storage.set(USERS_KEY, users);
}

async function seedIfEmpty(): Promise<void> {
  const existing = readTable();
  if (existing.length > 0) return;

  const seedUser: UserRecord = {
    id: crypto.randomUUID(),
    fullName: "Zainab Al-Kaabi",
    username: "zainab.basra",
    email: "zainab.kaabi@basra.gov.iq",
    nationalId: "19850234",
    passwordHash: await hashPassword("Basra@2026"),
    createdAt: new Date().toISOString(),
  };

  writeTable([seedUser]);
}

export const usersTable = {
  seedIfEmpty,
  findByUsername(username: string): UserRecord | undefined {
    return readTable().find(
      (user) => user.username.toLowerCase() === username.toLowerCase()
    );
  },
  findByEmail(email: string): UserRecord | undefined {
    return readTable().find((user) => {
      if (!user.email) return false;
      return user.email.toLowerCase() === email.toLowerCase();
    });
  },
  insert(user: UserRecord): void {
    const users = readTable();
    users.push(user);
    writeTable(users);
  },
};
