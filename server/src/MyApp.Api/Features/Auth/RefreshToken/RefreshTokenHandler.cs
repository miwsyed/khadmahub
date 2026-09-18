using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MyApp.Common;
using MyApp.Domain;
using MyApp.Features.Auth.Login;
using MyApp.Features.Auth.Shared;
using MyApp.Infrastructure;
using MyApp.Localization;
using DomainRefreshToken = MyApp.Domain.RefreshToken;

namespace MyApp.Features.Auth.RefreshToken;

public sealed class RefreshTokenHandler
{
    private readonly AppDbContext _dbContext;
    private readonly ITokenService _tokenService;
    private readonly TimeProvider _timeProvider;
    private readonly RefreshTokenOptions _refreshTokenOptions;
    private readonly ILogger<RefreshTokenHandler> _logger;

    public RefreshTokenHandler(
        AppDbContext dbContext,
        ITokenService tokenService,
        TimeProvider timeProvider,
        IOptions<RefreshTokenOptions> refreshTokenOptions,
        ILogger<RefreshTokenHandler> logger)
    {
        _dbContext = dbContext;
        _tokenService = tokenService;
        _timeProvider = timeProvider;
        _refreshTokenOptions = refreshTokenOptions.Value;
        _logger = logger;
    }

    public async Task<Result<LoginResponse>> HandleAsync(HttpContext httpContext, CancellationToken cancellationToken)
    {
        var cookieName = _refreshTokenOptions.CookieName;
        var refreshTokenValue = httpContext.Request.Cookies[cookieName];
        if (string.IsNullOrWhiteSpace(refreshTokenValue))
        {
            return Result<LoginResponse>.Failure(Error.Unauthorized("AUTH_AUTHENTICATION_REQUIRED"));
        }

        var now = _timeProvider.GetUtcNow();
        var tokenHash = RefreshTokenHasher.Hash(refreshTokenValue);
        var currentToken = await _dbContext.RefreshTokens
            .Include(token => token.User)
            .SingleOrDefaultAsync(token => token.TokenHash == tokenHash, cancellationToken);

        if (currentToken is null)
        {
            _logger.LogWarning("Refresh token not found for cookie {CookieName}", cookieName);
            return Result<LoginResponse>.Failure(Error.Unauthorized("AUTH_AUTHENTICATION_REQUIRED"));
        }

        if (currentToken.IsRevoked || currentToken.IsExpired(now))
        {
            await RevokeTokenFamilyAsync(currentToken, now, cancellationToken);
            _logger.LogWarning("Attempted refresh with revoked or expired token {TokenId}", currentToken.Id);
            return Result<LoginResponse>.Failure(Error.Unauthorized("AUTH_AUTHENTICATION_REQUIRED"));
        }

        currentToken.Revoke(now);

        var rotatedValue = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
        var rotatedHash = RefreshTokenHasher.Hash(rotatedValue);
        var rotatedToken = DomainRefreshToken.Create(
            currentToken.UserId,
            rotatedHash,
            now.AddDays(_refreshTokenOptions.LifetimeDays),
            currentToken.CreatedByIp,
            now);

        currentToken.ReplaceWith(rotatedToken.Id);
        _dbContext.RefreshTokens.Add(rotatedToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        httpContext.Response.Cookies.Append(
            cookieName,
            rotatedValue,
            new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = rotatedToken.ExpiresAtUtc.UtcDateTime,
                IsEssential = true,
                Path = "/"
            });

        var accessToken = _tokenService.CreateAccessToken(currentToken.User, now);
        var expiresAt = now.AddMinutes(15);
        var locale = RequestLocaleResolver.Resolve(httpContext);
        return Result<LoginResponse>.Success(
            new LoginResponse(
                accessToken,
                expiresAt,
                new LoginUserResponse(currentToken.User.Id, currentToken.User.Username, currentToken.User.FullName, currentToken.User.Role),
                MessageCatalog.Translate("AUTH_SESSION_REFRESHED", locale)));
    }

    private async Task RevokeTokenFamilyAsync(DomainRefreshToken rootToken, DateTimeOffset now, CancellationToken cancellationToken)
    {
        var familyIds = new HashSet<Guid> { rootToken.Id };
        var queue = new Queue<Guid>();
        queue.Enqueue(rootToken.Id);

        while (queue.Count > 0)
        {
            var currentId = queue.Dequeue();
            var descendants = await _dbContext.RefreshTokens
                .Where(token => token.UserId == rootToken.UserId && token.ReplacedByTokenId == currentId)
                .ToListAsync(cancellationToken);

            foreach (var descendant in descendants)
            {
                if (familyIds.Add(descendant.Id))
                {
                    queue.Enqueue(descendant.Id);
                }
            }
        }

        var tokens = await _dbContext.RefreshTokens
            .Where(token => familyIds.Contains(token.Id))
            .ToListAsync(cancellationToken);

        foreach (var token in tokens)
        {
            token.Revoke(now);
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
