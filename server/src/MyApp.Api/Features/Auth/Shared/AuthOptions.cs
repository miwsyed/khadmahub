using System.ComponentModel.DataAnnotations;

namespace MyApp.Features.Auth.Shared;

public sealed class JwtOptions
{
    public const string SectionName = "Jwt";

    [Required]
    public string Key { get; set; } = string.Empty;

    [Required]
    public string Issuer { get; set; } = "MyApp";

    [Required]
    public string Audience { get; set; } = "MyApp";

    [Range(1, int.MaxValue)]
    public int AccessTokenLifetimeMinutes { get; set; } = 15;
}

public sealed class LockoutOptions
{
    public const string SectionName = "Authentication:Lockout";

    [Range(1, int.MaxValue)]
    public int FailedAttemptsBeforeLockout { get; set; } = 5;

    [Range(1, int.MaxValue)]
    public int LockoutMinutes { get; set; } = 15;
}

public sealed class RefreshTokenOptions
{
    public const string SectionName = "Authentication:RefreshToken";

    [Range(1, int.MaxValue)]
    public int LifetimeDays { get; set; } = 7;

    [Required]
    public string CookieName { get; set; } = "myapp_refresh_token";
}

public sealed class AdminUserOptions
{
    public const string SectionName = "AdminUser";

    [Required]
    public string Username { get; set; } = "admin";

    [Required]
    public string Password { get; set; } = "Admin@123";
}
