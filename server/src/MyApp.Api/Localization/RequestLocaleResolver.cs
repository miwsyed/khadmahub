namespace MyApp.Localization;

public static class RequestLocaleResolver
{
    public static string Resolve(HttpContext? httpContext)
    {
        if (httpContext is null)
        {
            return "en";
        }

        foreach (var headerName in new[] { "X-Locale", "X-Language", "Accept-Language" })
        {
            var headerValue = httpContext.Request.Headers[headerName].FirstOrDefault();
            if (string.IsNullOrWhiteSpace(headerValue))
            {
                continue;
            }

            return MessageCatalog.NormalizeLocale(headerValue);
        }

        return "en";
    }
}
