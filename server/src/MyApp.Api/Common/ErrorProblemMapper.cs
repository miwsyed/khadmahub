using Microsoft.AspNetCore.Mvc;
using MyApp.Localization;

namespace MyApp.Common;

public static class ErrorProblemMapper
{
    public static IResult ToProblem(this Error error, HttpContext httpContext)
    {
        var locale = RequestLocaleResolver.Resolve(httpContext);
        var detail = MessageCatalog.Translate(error.Message, locale);
        var title = error.Type switch
        {
            ErrorType.Validation => MessageCatalog.Translate("AUTH_VALIDATION_FAILED", locale),
            ErrorType.Unauthorized => MessageCatalog.Translate("AUTH_AUTHENTICATION_REQUIRED", locale),
            ErrorType.Forbidden => MessageCatalog.Translate("AUTH_FORBIDDEN", locale),
            ErrorType.NotFound => MessageCatalog.Translate("AUTH_NOT_FOUND", locale),
            ErrorType.Conflict => MessageCatalog.Translate("AUTH_CONFLICT", locale),
            ErrorType.Lockout => MessageCatalog.Translate("AUTH_ACCOUNT_LOCKED", locale),
            ErrorType.RateLimit => MessageCatalog.Translate("AUTH_TOO_MANY_REQUESTS", locale),
            _ => MessageCatalog.Translate("AUTH_UNEXPECTED_ERROR", locale)
        };

        var problem = new ProblemDetails
        {
            Title = title,
            Status = MapStatus(error.Type),
            Detail = detail,
            Type = $"https://httpstatuses.com/{MapStatus(error.Type)}"
        };

        problem.Extensions["code"] = error.Code;
        problem.Extensions["traceId"] = httpContext.TraceIdentifier;
        problem.Extensions["locale"] = locale;

        if (error.Type == ErrorType.Validation)
        {
            return Results.ValidationProblem(new Dictionary<string, string[]>()
            {
                ["error"] = [detail]
            },
            title: problem.Title,
            detail: problem.Detail,
            statusCode: problem.Status,
            extensions: new Dictionary<string, object?>
            {
                ["code"] = error.Code,
                ["traceId"] = httpContext.TraceIdentifier,
                ["locale"] = locale
            });
        }

        return Results.Problem(
            title: problem.Title,
            detail: problem.Detail,
            statusCode: problem.Status,
            extensions: new Dictionary<string, object?>
            {
                ["code"] = error.Code,
                ["traceId"] = httpContext.TraceIdentifier,
                ["locale"] = locale
            });
    }

    private static int MapStatus(ErrorType type) => type switch
    {
        ErrorType.Validation => StatusCodes.Status400BadRequest,
        ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
        ErrorType.Forbidden => StatusCodes.Status403Forbidden,
        ErrorType.NotFound => StatusCodes.Status404NotFound,
        ErrorType.Conflict => StatusCodes.Status409Conflict,
        ErrorType.Lockout => StatusCodes.Status423Locked,
        ErrorType.RateLimit => StatusCodes.Status429TooManyRequests,
        _ => StatusCodes.Status500InternalServerError
    };
}
