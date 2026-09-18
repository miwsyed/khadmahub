using MyApp.Features.Auth.Login;

namespace MyApp.Features.Auth.SignUp;

public sealed record SignUpResponse(string? AccessToken, DateTimeOffset? ExpiresAt, LoginUserResponse User, string Message);
