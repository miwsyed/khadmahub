export interface ProblemDetailsPayload {
  detail?: string;
  title?: string;
  errors?: Record<string, string[]>;
}
