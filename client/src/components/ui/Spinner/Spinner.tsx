import type { SpinnerProps } from "./types/spinner.types";
import styles from "./Spinner.module.css";

export function Spinner({ size = 20 }: SpinnerProps) {
  return (
    <span
      className={styles.spinner}
      style={{ width: size, height: size }}
      role="presentation"
    />
  );
}
