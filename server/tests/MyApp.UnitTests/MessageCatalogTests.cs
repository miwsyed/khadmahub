using FluentAssertions;
using Microsoft.AspNetCore.Http;
using MyApp.Localization;
using Xunit;

namespace MyApp.UnitTests;

public sealed class MessageCatalogTests
{
    [Fact]
    public void TranslateShouldReturnArabicMessageForArabicLocale()
    {
        var message = MessageCatalog.Translate("AUTH_LOGIN_SUCCESS", "ar");

        message.Should().Be("تم تسجيل الدخول بنجاح.");
    }

    [Fact]
    public void ResolveShouldPreferExplicitLocaleHeader()
    {
        var httpContext = new DefaultHttpContext();
        httpContext.Request.Headers["X-Locale"] = "ku";

        var locale = RequestLocaleResolver.Resolve(httpContext);

        locale.Should().Be("ku");
    }
}
