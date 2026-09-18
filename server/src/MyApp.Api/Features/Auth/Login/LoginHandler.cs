using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MyApp.Common;
using MyApp.Domain;
using MyApp.Features.Auth.Shared;
using MyApp.Infrastructure;
using MyApp.Localization;
using DomainRefreshToken = MyApp.Domain.RefreshToken;

namespace MyApp.Features.Auth.Login;

public sealed class LoginHandler
{
    private readonly AppDbContext _dbContext;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenService _tokenService;
    private readonly TimeProvider _timeProvider;
    private readonly LockoutOptions _lockoutOptions;
    private readonly RefreshTokenOptions _refreshTokenOptions;
    private readonly ILogger<LoginHandler> _logger;

    public LoginHandler(
        AppDbContext dbContext,
        IPasswordHasher passwordHasher,
        ITokenService tokenService,
        TimeProvider timeProvider,
        IOptions<LockoutOptions> lockoutOptions,
        IOptions<RefreshTokenOptions> refreshTokenOptions,
        ILogger<LoginHandler> logger)
    {
        _dbContext = dbContext;
        _passwordHasher = passwordHasher;
        _tokenService = tokenService;
        _timeProvider = timeProvider;
        _lockoutOptions = lockoutOptions.Value;
        _refreshTokenOptions = refreshTokenOptions.Value;
        _logger = logger;
    }

    public async Task<Result<LoginResponse>> HandleAsync(LoginRequest request, HttpContext httpContext, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(httpContext);

        var now = _timeProvider.GetUtcNow();
        var username = request.Username.Trim();

        if (string.IsNullOrWhiteSpace(username))
        {
            return Result<LoginResponse>.Failure(Error.Validation("AUTH_USERNAME_REQUIRED", "AUTH_USERNAME_REQUIRED"));
        }

        var dummyPassword = "__login_dummy__";
        var dummyHash = _passwordHasher.Hash(dummyPassword);
        _ = _passwordHasher.Verify(dummyPassword, dummyHash);

        var user = await _dbContext.Users
            .AsNoTracking()
            .SingleOrDefaultAsync(existing => existing.Username == username, cancellationToken);

        if (user is null)
        {
            _logger.LogWarning("Login attempt for unknown username: {Username}", username);
            return Result<LoginResponse>.Failure(Error.Unauthorized("AUTH_INVALID_CREDENTIALS"));
        }

        if (!user.IsActive)
        {
            _logger.LogWarning("Login attempt for inactive user: {UserId}", user.Id);
            return Result<LoginResponse>.Failure(Error.Unauthorized("AUTH_INVALID_CREDENTIALS"));
        }

        if (user.IsLockedOut(now))
        {
            _logger.LogWarning("Login blocked due to lockout: {UserId}", user.Id);
            return Result<LoginResponse>.Failure(Error.Lockout("AUTH_ACCOUNT_LOCKED"));
        }

        var passwordValid = _passwordHasher.Verify(request.Password, user.PasswordHash);
        if (!passwordValid)
        {
            user.RegisterFailedLogin(now);

            if (user.FailedLoginCount >= _lockoutOptions.FailedAttemptsBeforeLockout)
            {
                _logger.LogWarning("User {UserId} reached lockout threshold.", user.Id);
            }

            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return Result<LoginResponse>.Failure(Error.Unauthorized("AUTH_INVALID_CREDENTIALS"));
        }

        user.RecordSuccessfulLogin(now);
        _dbContext.Users.Update(user);

        var accessToken = _tokenService.CreateAccessToken(user, now);
        var expiresAt = now.AddMinutes(15);
        var refreshTokenValue = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
        var refreshTokenHash = RefreshTokenHasher.Hash(refreshTokenValue);
        var refreshToken = DomainRefreshToken.Create(user.Id, refreshTokenHash, now.AddDays(_refreshTokenOptions.LifetimeDays), httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown", now);

        _dbContext.RefreshTokens.Add(refreshToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        httpContext.Response.Cookies.Append(
            _refreshTokenOptions.CookieName,
            refreshTokenValue,
            new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = refreshToken.ExpiresAtUtc.UtcDateTime,
                IsEssential = true,
                Path = "/"
            });

        var locale = RequestLocaleResolver.Resolve(httpContext);
        return Result<LoginResponse>.Success(
            new LoginResponse(
                accessToken,
                expiresAt,
                new LoginUserResponse(user.Id, user.Username, user.FullName, user.Role),
                MessageCatalog.Translate("AUTH_LOGIN_SUCCESS", locale)));
    }
}
