/**
 * Small typed wrapper around localStorage so feature modules never touch
 * the raw Web Storage API (and JSON parsing) directly.
 */
export const storage = {
  get<T>(key: string): T | null {
    const raw = window.localStorage.getItem(key);
    if (!raw) return null;
    try {
      return JSON.parse(raw) as T;
    } catch {
      return null;
    }
  },
  set<T>(key: string, value: T): void {
    window.localStorage.setItem(key, JSON.stringify(value));
  },
  remove(key: string): void {
    window.localStorage.removeItem(key);
  },
};
