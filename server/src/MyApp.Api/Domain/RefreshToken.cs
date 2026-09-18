using MyApp.Common;

namespace MyApp.Domain;

public sealed class RefreshToken
{
    private RefreshToken()
    {
    }

    public Guid Id { get; private set; } = GuidFactory.CreateVersion7();

    public Guid UserId { get; private set; }

    public string TokenHash { get; private set; } = string.Empty;

    public DateTimeOffset ExpiresAtUtc { get; private set; }

    public DateTimeOffset CreatedAtUtc { get; private set; }

    public string CreatedByIp { get; private set; } = string.Empty;

    public DateTimeOffset? RevokedAtUtc { get; private set; }

    public Guid? ReplacedByTokenId { get; private set; }

    public User User { get; private set; } = null!;

    public static RefreshToken Create(Guid userId, string tokenHash, DateTimeOffset expiresAtUtc, string createdByIp, DateTimeOffset now)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(tokenHash);
        ArgumentException.ThrowIfNullOrWhiteSpace(createdByIp);

        return new RefreshToken
        {
            UserId = userId,
            TokenHash = tokenHash,
            ExpiresAtUtc = expiresAtUtc,
            CreatedAtUtc = now,
            CreatedByIp = createdByIp
        };
    }

    public bool IsExpired(DateTimeOffset now) => ExpiresAtUtc <= now;

    public bool IsRevoked => RevokedAtUtc.HasValue;

    public void Revoke(DateTimeOffset now)
    {
        RevokedAtUtc ??= now;
    }

    public void ReplaceWith(Guid tokenId)
    {
        ReplacedByTokenId = tokenId;
    }
}
