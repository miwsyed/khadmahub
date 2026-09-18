using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Testcontainers.MsSql;
using Xunit;

namespace MyApp.IntegrationTests;

public sealed class AuthEndpointsTests : IClassFixture<WebApplicationFactory<Program>>, IDisposable
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly MsSqlContainer _sqlContainer;

    public AuthEndpointsTests()
    {
        _factory = new WebApplicationFactory<Program>();
        _sqlContainer = new MsSqlBuilder()
            .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
            .WithPassword("YourStrong!Passw0rd")
            .Build();
    }

    [Fact]
    public async Task SignUpShouldCreateUserWithoutAutoLoginAndAllowOptionalNationalId()
    {
        using var client = _factory.CreateClient();

        var response = await client.PostAsJsonAsync("/api/v1/auth/sign-up", new
        {
            fullName = "Alice Example",
            username = "alice.example",
            email = "alice@example.com",
            password = "StrongPass123"
        });

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var payload = await response.Content.ReadFromJsonAsync<SignUpPayloadResponse>();
        payload.Should().NotBeNull();
        payload!.AccessToken.Should().BeNullOrEmpty();
        payload.User.Username.Should().Be("alice.example");
        payload.User.FullName.Should().Be("Alice Example");
    }

    [Fact(Skip = "Requires a local Docker engine for Testcontainers.")]
    public async Task LoginShouldReturnUnauthorizedWhenPasswordIsWrong()
    {
        await _sqlContainer.StartAsync();

        using var client = _factory.CreateClient();
        var response = await client.PostAsJsonAsync("/api/v1/auth/login", new { username = "admin", password = "WrongPassword" });

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

#pragma warning disable CA1812
    private sealed record SignUpPayloadResponse
    {
        public string AccessToken { get; init; } = string.Empty;
        public SignUpUserResponse User { get; init; } = new();
    }

    private sealed record SignUpUserResponse
    {
        public string Username { get; init; } = string.Empty;
        public string FullName { get; init; } = string.Empty;
    }
#pragma warning restore CA1812

    public void Dispose()
    {
        _factory.Dispose();
        _sqlContainer.DisposeAsync().AsTask().GetAwaiter().GetResult();
    }
}
