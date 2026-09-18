using FluentAssertions;
using MyApp.Domain;
using Xunit;

namespace MyApp.UnitTests;

public sealed class UserTests
{
    [Fact]
    public void CreateShouldSetDefaultsAndAuditFields()
    {
        var now = DateTimeOffset.UtcNow;

        var user = User.Create("admin", "hash", "Admin User", "Admin", now);

        user.Id.Should().NotBeEmpty();
        user.Username.Should().Be("admin");
        user.FullName.Should().Be("Admin User");
        user.Role.Should().Be("Admin");
        user.CreatedAtUtc.Should().Be(now);
        user.UpdatedAtUtc.Should().Be(now);
    }

    [Fact]
    public void RegisterFailedLoginShouldIncrementAndLockOutAfterThreshold()
    {
        var now = DateTimeOffset.UtcNow;
        var user = User.Create("john", "hash", "John Doe", "User", now);

        for (var i = 0; i < 4; i++)
        {
            user.RegisterFailedLogin(now); 
        }

        user.IsLockedOut(now).Should().BeFalse();
        user.RegisterFailedLogin(now);
        user.IsLockedOut(now).Should().BeTrue();
        user.FailedLoginCount.Should().Be(5);
    }

    [Fact]
    public void RecordSuccessfulLoginShouldResetFailures()
    {
        var now = DateTimeOffset.UtcNow;
        var user = User.Create("john", "hash", "John Doe", "User", now);

        user.RegisterFailedLogin(now);
        user.RecordSuccessfulLogin(now.AddMinutes(1));

        user.FailedLoginCount.Should().Be(0);
        user.LastLoginAtUtc.Should().NotBeNull();
    }
}
