# MyApp backend

This repository contains the initial backend for the MyApp platform, with the first deliverable focused on the authentication flow.

## Prerequisites

- .NET SDK 8.0.100
- Docker Desktop (for SQL Server and container-based runs)
- Optional: SQL Server Management Studio or `sqlcmd`

## Local startup

1. Copy `.env.example` to `.env` and adjust the values if needed.
2. Start the database and API:
   
   ```bash
   docker compose up --build
   ```

3. The API listens on `http://localhost:8080` in development.

## Run tests

```bash
cd backend
C:\Users\syedn\.dotnet\dotnet.exe test MyApp.sln --nologo
```

## Build

```bash
cd backend
C:\Users\syedn\.dotnet\dotnet.exe build MyApp.sln -warnaserror --nologo
```

## Adding a new feature

1. Create a new folder under `src/MyApp.Api/Features/<FeatureName>/`.
2. Add the endpoint, request/response DTOs, validator, and handler in the same slice.
3. Put shared domain concepts in `src/MyApp.Api/Domain` or a `Common` utility abstraction.
4. Implement `IEndpoint` and return `Results.*` with the shared `Result<T>` error pattern.
5. Ensure the endpoint is discovered automatically by the assembly scan in `EndpointDiscovery`.

## Migration workflow

```bash
cd backend
C:\Users\syedn\.dotnet\dotnet.exe ef migrations add <MigrationName> --project src/MyApp.Api/MyApp.Api.csproj --startup-project src/MyApp.Api/MyApp.Api.csproj
C:\Users\syedn\.dotnet\dotnet.exe ef database update --project src/MyApp.Api/MyApp.Api.csproj --startup-project src/MyApp.Api/MyApp.Api.csproj
```

Production deployments should run migrations explicitly through a controlled release pipeline rather than auto-migrating on startup.
