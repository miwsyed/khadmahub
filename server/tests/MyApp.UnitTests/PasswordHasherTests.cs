using FluentAssertions;
using MyApp.Features.Auth.Shared;
using Xunit;

namespace MyApp.UnitTests;

public sealed class PasswordHasherTests
{
    [Fact]
    public void BCryptPasswordHasherShouldVerifyKnownPassword()
    {
        var hasher = new BCryptPasswordHasher();
        var password = "Admin@123";

        var hash = hasher.Hash(password);

        hasher.Verify(password, hash).Should().BeTrue();
        hasher.Verify("WrongPassword", hash).Should().BeFalse();
    }
}
