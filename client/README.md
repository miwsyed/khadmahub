# KhadmaHub — Government of Basra e-Services (Scaffold)

A feature-based React 19 + TypeScript scaffold for a Basra government
services portal: authentication (sign in / sign up), route guarding via
higher-order components, a navbar + sidebar app shell, and a placeholder
dashboard of directorate services.

This is intentionally scoped to the auth flow and shell only, as requested —
no additional modules have been built beyond what's needed to demonstrate
the pattern end to end.

## Stack

- **React 19** + **TypeScript**, built with **Vite**
- **react-router-dom** for routing
- **react-hook-form** + **zod** for form state and validation
- CSS Modules with a small design-token system (`src/styles/tokens.css`) —
  no UI framework, so the look is bespoke rather than default component-kit
  styling
- A mock "Users table" persisted in `localStorage`, with passwords hashed
  client-side via `crypto.subtle` (SHA-256) before storage

## Getting started

```bash
npm install
npm run dev
```

The app opens at `http://localhost:5173` and redirects to `/login`.

### Demo account

A user is seeded automatically on first run:

| Field    | Value          |
| -------- | -------------- |
| Username | `zainab.basra` |
| Password | `Basra@2026`   |

You can also register a new account from the **Create an account** link on
the login screen — it's written to the same mock table.

## Folder structure

The project is organized by **feature**, not by file type, so each domain
owns its own components, pages, hooks, API calls and types:

```
src/
├── app/                    # App-wide wiring
│   ├── providers/          # Context provider composition (Router, Toast, Auth)
│   └── routes/             # Route table, wraps pages with HOC guards
├── components/
│   ├── layout/              # Navbar, Sidebar, MainLayout, AuthLayout
│   └── ui/                  # Reusable primitives: Button, Input, Toast, Spinner...
├── context/                 # AuthContext, ToastContext
├── hoc/                     # withAuthGuard, withGuestGuard
├── features/
│   ├── auth/
│   │   ├── api/              # authApi.ts (mock REST layer) + mockDatabase.ts
│   │   ├── components/       # LoginForm, SignUpForm
│   │   ├── pages/            # LoginPage, SignUpPage
│   │   ├── schemas/          # zod validation schemas
│   │   └── types/            # Auth domain types
│   └── dashboard/
│       ├── components/       # ServiceCard
│       ├── data/             # Static service catalog
│       └── pages/            # HomePage
├── lib/                     # storage.ts, delay.ts — small framework-agnostic helpers
└── styles/                  # tokens.css (design system), globals.css
```

## Replacing the mock API with a real backend

Everything the UI needs goes through `src/features/auth/api/authApi.ts`.
To connect a real backend:

1. Delete `mockDatabase.ts`.
2. Replace the body of `authApi.login` and `authApi.signUp` with `fetch`
   calls to your endpoints (e.g. `POST /api/auth/login`,
   `POST /api/auth/register`).
3. Nothing in `AuthContext`, the forms, or the route guards needs to change —
   they only depend on the `AuthResult` shape returned by `authApi`.

## Routing & guards

`AppRoutes.tsx` wraps each page in a higher-order component:

- `withGuestGuard` — used on `/login` and `/sign-up`; redirects an
  already-authenticated user straight to `/home`.
- `withAuthGuard` — used on `/home`; redirects an anonymous visitor to
  `/login` and remembers where they were headed.

## Notes on the design

The palette and layout are drawn from the subject matter rather than a
generic template: a deep Shatt-al-Arab teal, riverside sand, an
official-seal brick red, and brass for civic accents; a split-screen sign-in
inspired by government portal conventions, with a hand-built seal mark and
geometric lattice pattern instead of stock imagery or icon-kit logos.
