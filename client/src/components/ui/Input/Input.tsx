import { forwardRef, useId } from "react";
import clsx from "clsx";
import type { InputProps } from "../types/input.types";
import styles from "./Input.module.css";

export const Input = forwardRef<HTMLInputElement, InputProps>(
  ({ label, error, hint, id, className, ...rest }, ref) => {
    const generatedId = useId();
    const inputId = id ?? generatedId;
    const errorId = `${inputId}-error`;
    const hintId = `${inputId}-hint`;

    return (
      <div className={clsx(styles.field, className)}>
        <label htmlFor={inputId} className={styles.label}>
          {label}
        </label>
        <input
          ref={ref}
          id={inputId}
          className={clsx(styles.input, error && styles.inputError)}
          aria-invalid={Boolean(error)}
          aria-describedby={error ? errorId : hint ? hintId : undefined}
          {...rest}
        />
        {error ? (
          <p id={errorId} className={styles.errorText}>
            {error}
          </p>
        ) : hint ? (
          <p id={hintId} className={styles.hintText}>
            {hint}
          </p>
        ) : null}
      </div>
    );
  }
);

Input.displayName = "Input";
