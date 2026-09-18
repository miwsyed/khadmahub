using FluentAssertions;
using Microsoft.Extensions.Options;
using MyApp.Domain;
using MyApp.Features.Auth.Shared;
using Xunit;

namespace MyApp.UnitTests;

public sealed class TokenServiceTests
{
    [Fact]
    public void CreateAccessTokenShouldReturnTokenForValidUser()
    {
        var options = Options.Create(new JwtOptions
        {
            Key = "this-is-a-test-key-which-must-be-at-least-32-bytes-long",
            Issuer = "MyApp",
            Audience = "MyApp",
            AccessTokenLifetimeMinutes = 15
        });

        var user = User.Create("admin", "hash", "Admin User", "Admin", DateTimeOffset.UtcNow);
        var service = new TokenService(options, TimeProvider.System);

        var token = service.CreateAccessToken(user, DateTimeOffset.UtcNow);

        token.Should().NotBeNullOrWhiteSpace();
        token.Length.Should().BeGreaterThan(20);
    }
}
