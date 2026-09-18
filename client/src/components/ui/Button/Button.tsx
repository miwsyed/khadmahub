import { forwardRef } from "react";
import clsx from "clsx";
import { Spinner } from "@/components/ui/Spinner/Spinner";
import type { ButtonProps } from "../types/button.types";
import styles from "./Button.module.css";

export const Button = forwardRef<HTMLButtonElement, ButtonProps>(
  (
    {
      variant = "primary",
      fullWidth = false,
      isLoading = false,
      disabled,
      className,
      children,
      ...rest
    },
    ref
  ) => {
    return (
      <button
        ref={ref}
        className={clsx(
          styles.button,
          styles[variant],
          fullWidth && styles.fullWidth,
          className
        )}
        disabled={disabled || isLoading}
        aria-busy={isLoading}
        {...rest}
      >
        {isLoading ? <Spinner size={16} /> : null}
        <span>{children}</span>
      </button>
    );
  }
);

Button.displayName = "Button";
