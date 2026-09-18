using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using MyApp.Common;
using MyApp.Infrastructure;

namespace MyApp.Features.Auth.Me;

public sealed class MeEndpoint : IEndpoint
{
    public void Map(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v1/auth");

        group.MapGet("/me", async (ClaimsPrincipal user, AppDbContext dbContext, CancellationToken cancellationToken) =>
            {
                var userIdClaim = user.FindFirstValue(ClaimTypes.NameIdentifier);
                if (!Guid.TryParse(userIdClaim, out var userId))
                {
                    return Results.Problem(statusCode: StatusCodes.Status401Unauthorized, title: "Unauthorized", detail: "Authentication required.");
                }

                var currentUser = await dbContext.Users
                    .AsNoTracking()
                    .SingleOrDefaultAsync(existing => existing.Id == userId, cancellationToken);

                if (currentUser is null)
                {
                    return Results.Problem(statusCode: StatusCodes.Status401Unauthorized, title: "Unauthorized", detail: "Authentication required.");
                }

                return Results.Ok(new MeResponse(currentUser.Id, currentUser.Username, currentUser.FullName, currentUser.Role));
            })
            .RequireAuthorization()
            .WithName("GetCurrentUser")
            .Produces<MeResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status401Unauthorized);
    }
}
