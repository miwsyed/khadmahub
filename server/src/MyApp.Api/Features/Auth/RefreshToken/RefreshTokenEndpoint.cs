using MyApp.Common;
using MyApp.Features.Auth.Login;

namespace MyApp.Features.Auth.RefreshToken;

public sealed class RefreshTokenEndpoint : IEndpoint
{
    public void Map(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v1/auth");

        group.MapPost("/refresh", async (HttpContext httpContext, RefreshTokenHandler handler, CancellationToken cancellationToken) =>
            {
                var result = await handler.HandleAsync(httpContext, cancellationToken);
                if (result.IsFailure)
                {
                    return result.Error.ToProblem(httpContext);
                }

                return Results.Ok(result.Value!);
            })
            .AllowAnonymous()
            .RequireRateLimiting("refresh-policy")
            .WithName("RefreshToken")
            .Produces<LoginResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status429TooManyRequests)
            .ProducesProblem(StatusCodes.Status423Locked);
    }
}
