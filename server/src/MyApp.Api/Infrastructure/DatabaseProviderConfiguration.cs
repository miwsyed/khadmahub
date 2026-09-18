using Microsoft.EntityFrameworkCore;

namespace MyApp.Infrastructure;

public static class DatabaseProviderConfiguration
{
    public static void Configure(DbContextOptionsBuilder options, string? connectionString)
    {
        var resolvedConnectionString = ResolveConnectionString(connectionString);

        if (IsSqliteConnectionString(resolvedConnectionString))
        {
            options.UseSqlite(resolvedConnectionString);
            return;
        }

        options.UseSqlServer(resolvedConnectionString);
    }

    public static string ResolveConnectionString(string? connectionString)
    {
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            return "Data Source=MyAppDb.db";
        }

        var trimmed = connectionString.Trim();
        if (IsSqliteConnectionString(trimmed))
        {
            return trimmed;
        }

        var isLocalSqlServerConnection = trimmed.Contains("Server=localhost", StringComparison.OrdinalIgnoreCase)
            || trimmed.Contains("Server=127.0.0.1", StringComparison.OrdinalIgnoreCase)
            || trimmed.Contains("Data Source=localhost", StringComparison.OrdinalIgnoreCase)
            || trimmed.Contains("Data Source=127.0.0.1", StringComparison.OrdinalIgnoreCase);

        return isLocalSqlServerConnection ? "Data Source=MyAppDb.db" : trimmed;
    }

    public static bool IsSqliteConnectionString(string? connectionString)
    {
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            return false;
        }

        return connectionString.Contains("Data Source=", StringComparison.OrdinalIgnoreCase)
            || connectionString.Contains("Filename=", StringComparison.OrdinalIgnoreCase)
            || connectionString.Contains("DataSource=", StringComparison.OrdinalIgnoreCase);
    }
}
