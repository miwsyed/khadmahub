namespace MyApp.Features.Auth.Login;

public sealed record LoginResponse(string AccessToken, DateTimeOffset ExpiresAt, LoginUserResponse User, string Message);

public sealed record LoginUserResponse(Guid Id, string Username, string FullName, string Role);
