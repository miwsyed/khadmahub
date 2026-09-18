namespace MyApp.Features.Auth.SignUp;

public sealed record SignUpRequest(string FullName, string Username, string Email, string? NationalId, string Password);
