namespace MyApp.Features.Auth.Me;

public sealed record MeResponse(Guid Id, string Username, string FullName, string Role);
