using MyApp.Common;

namespace MyApp.Domain;

public sealed class User
{
    private User()
    {
    }

    public Guid Id { get; private set; } = GuidFactory.CreateVersion7();

    public string Username { get; private set; } = string.Empty;

    public string PasswordHash { get; private set; } = string.Empty;

    public string FullName { get; private set; } = string.Empty;

    public string Email { get; private set; } = string.Empty;

    public string? NationalId { get; private set; }

    public string Role { get; private set; } = "User";

    public bool IsActive { get; private set; }

    public int FailedLoginCount { get; private set; }

    public DateTimeOffset? LockoutEndUtc { get; private set; }

    public DateTimeOffset? LastLoginAtUtc { get; private set; }

    public DateTimeOffset CreatedAtUtc { get; private set; }

    public DateTimeOffset UpdatedAtUtc { get; private set; }

    public static User Create(string username, string passwordHash, string fullName, string role, DateTimeOffset now, bool isActive = true, string? email = null, string? nationalId = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(username);
        ArgumentException.ThrowIfNullOrWhiteSpace(passwordHash);
        ArgumentException.ThrowIfNullOrWhiteSpace(fullName);

        return new User
        {
            Username = username.Trim(),
            PasswordHash = passwordHash,
            FullName = fullName.Trim(),
            Email = email?.Trim() ?? string.Empty,
            NationalId = string.IsNullOrWhiteSpace(nationalId) ? null : nationalId.Trim(),
            Role = string.IsNullOrWhiteSpace(role) ? "User" : role,
            IsActive = isActive,
            CreatedAtUtc = now,
            UpdatedAtUtc = now
        };
    }

    public bool IsLockedOut(DateTimeOffset now)
    {
        return LockoutEndUtc.HasValue && LockoutEndUtc.Value > now;
    }

    public void RegisterFailedLogin(DateTimeOffset now)
    {
        if (IsLockedOut(now))
        {
            return;
        }

        FailedLoginCount++;

        if (FailedLoginCount >= 5)
        {
            LockoutEndUtc = now.AddMinutes(15);
        }

        UpdatedAtUtc = now;
    }

    public void ResetFailures(DateTimeOffset now)
    {
        FailedLoginCount = 0;
        LockoutEndUtc = null;
        UpdatedAtUtc = now;
    }

    public void RecordSuccessfulLogin(DateTimeOffset now)
    {
        ResetFailures(now);
        LastLoginAtUtc = now;
        UpdatedAtUtc = now;
    }

    public void SetPasswordHash(string passwordHash, DateTimeOffset now)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(passwordHash);

        PasswordHash = passwordHash;
        UpdatedAtUtc = now;
    }
}
