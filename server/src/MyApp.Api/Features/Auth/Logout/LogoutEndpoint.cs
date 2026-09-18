using MyApp.Common;

namespace MyApp.Features.Auth.Logout;

public sealed class LogoutEndpoint : IEndpoint
{
    public void Map(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v1/auth");

        group.MapPost("/logout", async (HttpContext httpContext, LogoutHandler handler, CancellationToken cancellationToken) =>
            {
                var result = await handler.HandleAsync(httpContext, cancellationToken);
                if (result.IsFailure)
                {
                    return result.Error.ToProblem(httpContext);
                }

                return Results.NoContent();
            })
            .AllowAnonymous()
            .WithName("LogoutUser")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status401Unauthorized);
    }
}
