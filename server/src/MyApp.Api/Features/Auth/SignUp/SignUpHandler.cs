using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MyApp.Common;
using MyApp.Domain;
using MyApp.Features.Auth.Login;
using MyApp.Features.Auth.Shared;
using MyApp.Infrastructure;
using MyApp.Localization;

namespace MyApp.Features.Auth.SignUp;

public sealed class SignUpHandler
{
    private readonly AppDbContext _dbContext;
    private readonly IPasswordHasher _passwordHasher;
    private readonly TimeProvider _timeProvider;
    private readonly ILogger<SignUpHandler> _logger;

    public SignUpHandler(
        AppDbContext dbContext,
        IPasswordHasher passwordHasher,
        TimeProvider timeProvider,
        ILogger<SignUpHandler> logger)
    {
        _dbContext = dbContext;
        _passwordHasher = passwordHasher;
        _timeProvider = timeProvider;
        _logger = logger;
    }

    public async Task<Result<SignUpResponse>> HandleAsync(SignUpRequest request, HttpContext httpContext, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(httpContext);

        var now = _timeProvider.GetUtcNow();
        var fullName = request.FullName.Trim();
        var username = request.Username.Trim();
        var email = request.Email.Trim();
        var nationalId = request.NationalId?.Trim();

        if (string.IsNullOrWhiteSpace(fullName))
        {
            return Result<SignUpResponse>.Failure(Error.Validation("AUTH_FULL_NAME_REQUIRED", "AUTH_FULL_NAME_REQUIRED"));
        }

        if (string.IsNullOrWhiteSpace(username))
        {
            return Result<SignUpResponse>.Failure(Error.Validation("AUTH_USERNAME_REQUIRED", "AUTH_USERNAME_REQUIRED"));
        }

        if (string.IsNullOrWhiteSpace(email) || !email.Contains('@', StringComparison.Ordinal))
        {
            return Result<SignUpResponse>.Failure(Error.Validation("AUTH_EMAIL_REQUIRED", "AUTH_EMAIL_REQUIRED"));
        }

        var usernameExists = await _dbContext.Users.AsNoTracking().AnyAsync(user => user.Username == username, cancellationToken);
        if (usernameExists)
        {
            _logger.LogWarning("Signup failed because username already exists: {Username}", username);
            return Result<SignUpResponse>.Failure(Error.Conflict("AUTH_USERNAME_TAKEN"));
        }

        var emailExists = await _dbContext.Users.AsNoTracking().AnyAsync(user => user.Email == email, cancellationToken);
        if (emailExists)
        {
            _logger.LogWarning("Signup failed because email already exists: {Email}", email);
            return Result<SignUpResponse>.Failure(Error.Conflict("AUTH_EMAIL_TAKEN"));
        }

        if (!string.IsNullOrWhiteSpace(nationalId))
        {
            var nationalIdExists = await _dbContext.Users.AsNoTracking().AnyAsync(user => user.NationalId == nationalId, cancellationToken);
            if (nationalIdExists)
            {
                _logger.LogWarning("Signup failed because national ID already exists: {NationalId}", nationalId);
                return Result<SignUpResponse>.Failure(Error.Conflict("AUTH_NATIONAL_ID_TAKEN"));
            }
        }

        var passwordHash = _passwordHasher.Hash(request.Password);
        var user = User.Create(username, passwordHash, fullName, "User", now, true, email, nationalId);

        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync(cancellationToken);

        var locale = RequestLocaleResolver.Resolve(httpContext);
        return Result<SignUpResponse>.Success(
            new SignUpResponse(
                null,
                null,
                new LoginUserResponse(user.Id, user.Username, user.FullName, user.Role),
                MessageCatalog.Translate("AUTH_SIGNUP_SUCCESS", locale)));
    }
}
