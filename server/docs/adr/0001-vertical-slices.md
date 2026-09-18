# ADR 0001: Vertical slices for feature growth

- Status: Accepted
- Date: 2026-09-18

## Context

The backend will grow into a large enterprise application. A layered design would spread one feature across many folders and create hidden coupling.

## Decision

Organize the codebase by feature slice, with each feature owning its endpoint, validator, handler, DTOs, and related persistence configuration.

## Consequences

- Adding a feature is mostly local and safe.
- Shared concepts live in `Domain` or `Common` to avoid duplication.
- The assembly scan keeps the API startup almost untouched.
