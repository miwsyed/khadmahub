# ADR 0002: No mediator library

- Status: Accepted
- Date: 2026-09-18

## Context

The team wants explicit behavior, low cognitive overhead, and predictable middleware composition without a large abstraction layer.

## Decision

Use explicit handlers and endpoint methods, not MediatR or AutoMapper.

## Consequences

- Dependencies remain obvious.
- DI and unit tests are straightforward.
- Feature code stays close to the business behavior it serves.
