using FluentValidation;
using MyApp.Localization;

namespace MyApp.Common;

public sealed class ValidationEndpointFilter<TRequest> : IEndpointFilter
    where TRequest : class
{
    private readonly IValidator<TRequest> _validator;

    public ValidationEndpointFilter(IValidator<TRequest> validator)
    {
        _validator = validator;
    }

    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var request = context.Arguments.OfType<TRequest>().FirstOrDefault();
        if (request is null)
        {
            return await next(context);
        }

        var validationResult = await _validator.ValidateAsync(request, context.HttpContext.RequestAborted);
        if (!validationResult.IsValid)
        {
            var locale = RequestLocaleResolver.Resolve(context.HttpContext);
            var errors = validationResult.Errors
                .GroupBy(failure => failure.PropertyName)
                .ToDictionary(
                    group => group.Key,
                    group => group.Select(failure => MessageCatalog.Translate(failure.ErrorMessage, locale)).ToArray());

            return Results.ValidationProblem(
                errors,
                title: MessageCatalog.Translate("AUTH_VALIDATION_FAILED", locale),
                statusCode: StatusCodes.Status400BadRequest,
                extensions: new Dictionary<string, object?>
                {
                    ["locale"] = locale
                });
        }

        return await next(context);
    }
}
