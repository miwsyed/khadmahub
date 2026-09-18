using MyApp.Common;
using MyApp.Features.Auth.Login;

namespace MyApp.Features.Auth.SignUp;

public sealed class SignUpEndpoint : IEndpoint
{
    public void Map(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v1/auth");

        group.MapPost("/sign-up", async (SignUpRequest request, HttpContext httpContext, SignUpHandler handler, CancellationToken cancellationToken) =>
            {
                var result = await handler.HandleAsync(request, httpContext, cancellationToken);
                if (result.IsFailure)
                {
                    return result.Error.ToProblem(httpContext);
                }

                return Results.Ok(result.Value!);
            })
            .AllowAnonymous()
            .AddEndpointFilter<ValidationEndpointFilter<SignUpRequest>>()
            .WithName("SignUpUser")
            .Produces<SignUpResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .ProducesProblem(StatusCodes.Status429TooManyRequests);
    }
}
