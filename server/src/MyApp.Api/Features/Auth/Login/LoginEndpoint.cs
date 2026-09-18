using FluentValidation;
using MyApp.Common;
using MyApp.Features.Auth.Shared;

namespace MyApp.Features.Auth.Login;

public sealed class LoginEndpoint : IEndpoint
{
    public void Map(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v1/auth");

        group.MapPost("/login", async (LoginRequest request, HttpContext httpContext, LoginHandler handler, CancellationToken cancellationToken) =>
            {
                var result = await handler.HandleAsync(request, httpContext, cancellationToken);
                if (result.IsFailure)
                {
                    return result.Error.ToProblem(httpContext);
                }

                return Results.Ok(result.Value!);
            })
            .AllowAnonymous()
            .RequireRateLimiting("login-policy")
            .AddEndpointFilter<ValidationEndpointFilter<LoginRequest>>()
            .WithName("LoginUser")
            .Produces<LoginResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status423Locked)
            .ProducesProblem(StatusCodes.Status429TooManyRequests);
    }
}
