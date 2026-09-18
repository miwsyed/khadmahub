/** Simulates network latency so loading states feel real against the mock API. */
export const delay = (ms: number): Promise<void> =>
  new Promise((resolve) => setTimeout(resolve, ms));
