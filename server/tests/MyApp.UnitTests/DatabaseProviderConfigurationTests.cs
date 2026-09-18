using FluentAssertions;
using MyApp.Infrastructure;
using Xunit;

namespace MyApp.UnitTests;

public class DatabaseProviderConfigurationTests
{
    [Theory]
    [InlineData("Data Source=myapp.db", true)]
    [InlineData("Filename=myapp.db", true)]
    [InlineData("Server=localhost,1433;Database=MyAppDb;User Id=sa;Password=Passw0rd!;TrustServerCertificate=True;Encrypt=False;", false)]
    public void DetectsSqliteConnectionString(string connectionString, bool expected)
    {
        DatabaseProviderConfiguration.IsSqliteConnectionString(connectionString).Should().Be(expected);
    }

    [Theory]
    [InlineData(null, "Data Source=MyAppDb.db")]
    [InlineData("", "Data Source=MyAppDb.db")]
    [InlineData("Server=localhost,1433;Database=MyAppDb;User Id=sa;Password=Passw0rd!;TrustServerCertificate=True;Encrypt=False;", "Data Source=MyAppDb.db")]
    [InlineData("Data Source=myapp.db", "Data Source=myapp.db")]
    public void ResolvesLocalDevelopmentConnectionString(string? connectionString, string expected)
    {
        DatabaseProviderConfiguration.ResolveConnectionString(connectionString).Should().Be(expected);
    }
}
