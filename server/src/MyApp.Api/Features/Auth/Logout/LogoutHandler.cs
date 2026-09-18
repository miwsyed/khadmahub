using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MyApp.Common;
using MyApp.Features.Auth.Shared;
using MyApp.Infrastructure;

namespace MyApp.Features.Auth.Logout;

public sealed class LogoutHandler
{
    private readonly AppDbContext _dbContext;
    private readonly RefreshTokenOptions _refreshTokenOptions;
    private readonly TimeProvider _timeProvider;

    public LogoutHandler(
        AppDbContext dbContext,
        IOptions<RefreshTokenOptions> refreshTokenOptions,
        TimeProvider timeProvider)
    {
        _dbContext = dbContext;
        _refreshTokenOptions = refreshTokenOptions.Value;
        _timeProvider = timeProvider;
    }

    public async Task<Result> HandleAsync(HttpContext httpContext, CancellationToken cancellationToken)
    {
        var cookieValue = httpContext.Request.Cookies[_refreshTokenOptions.CookieName];
        if (string.IsNullOrWhiteSpace(cookieValue))
        {
            httpContext.Response.Cookies.Delete(_refreshTokenOptions.CookieName, new CookieOptions { Path = "/", Secure = true, SameSite = SameSiteMode.Strict, HttpOnly = true });
            return Result.Success();
        }

        var tokenHash = RefreshTokenHasher.Hash(cookieValue);
        var currentToken = await _dbContext.RefreshTokens
            .SingleOrDefaultAsync(token => token.TokenHash == tokenHash, cancellationToken);

        if (currentToken is not null)
        {
            currentToken.Revoke(_timeProvider.GetUtcNow());
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        httpContext.Response.Cookies.Delete(_refreshTokenOptions.CookieName, new CookieOptions { Path = "/", Secure = true, SameSite = SameSiteMode.Strict, HttpOnly = true });
        return Result.Success();
    }
}
