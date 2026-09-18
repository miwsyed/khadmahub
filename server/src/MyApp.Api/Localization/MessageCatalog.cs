using System.Collections.Concurrent;

namespace MyApp.Localization;

public static class MessageCatalog
{
    private static readonly ConcurrentDictionary<string, string> Cache = new(StringComparer.Ordinal);

    private static readonly Dictionary<string, string> English = new(StringComparer.Ordinal)
    {
        ["AUTH_LOGIN_SUCCESS"] = "Login successful.",
        ["AUTH_SIGNUP_SUCCESS"] = "Sign up successful.",
        ["AUTH_SESSION_REFRESHED"] = "Session refreshed.",
        ["AUTH_INVALID_CREDENTIALS"] = "Incorrect username or password.",
        ["AUTH_USERNAME_REQUIRED"] = "Username is required.",
        ["AUTH_PASSWORD_REQUIRED"] = "Password is required.",
        ["AUTH_EMAIL_REQUIRED"] = "A valid email is required.",
        ["AUTH_FULL_NAME_REQUIRED"] = "Full name is required.",
        ["AUTH_ACCOUNT_LOCKED"] = "Your account is temporarily locked. Please try again later.",
        ["AUTH_USERNAME_TAKEN"] = "That username is already registered.",
        ["AUTH_EMAIL_TAKEN"] = "An account already exists for that email.",
        ["AUTH_NATIONAL_ID_TAKEN"] = "An account already exists for that national ID.",
        ["AUTH_USERNAME_PATTERN"] = "Username may only contain letters, numbers, dots, and underscores.",
        ["AUTH_PASSWORD_MIN_LENGTH"] = "Password must be at least 8 characters.",
        ["AUTH_PASSWORD_UPPERCASE"] = "Password must include at least one uppercase letter.",
        ["AUTH_PASSWORD_NUMBER"] = "Password must include at least one number.",
        ["AUTH_VALIDATION_FAILED"] = "Validation failed.",
        ["AUTH_CONFIRM_PASSWORD_MISMATCH"] = "Passwords do not match.",
        ["AUTH_NATIONAL_ID_FORMAT"] = "National ID must contain between 8 and 12 digits.",
        ["AUTH_AUTHENTICATION_REQUIRED"] = "Authentication required.",
        ["AUTH_FORBIDDEN"] = "Forbidden.",
        ["AUTH_NOT_FOUND"] = "Resource not found.",
        ["AUTH_CONFLICT"] = "Conflict.",
        ["AUTH_TOO_MANY_REQUESTS"] = "Too many requests.",
        ["AUTH_UNEXPECTED_ERROR"] = "An unexpected error occurred."
    };

    private static readonly Dictionary<string, string> Arabic = new(StringComparer.Ordinal)
    {
        ["AUTH_LOGIN_SUCCESS"] = "تم تسجيل الدخول بنجاح.",
        ["AUTH_SIGNUP_SUCCESS"] = "تم إنشاء الحساب بنجاح.",
        ["AUTH_SESSION_REFRESHED"] = "تم تجديد الجلسة.",
        ["AUTH_INVALID_CREDENTIALS"] = "اسم المستخدم أو كلمة المرور غير صحيحة.",
        ["AUTH_USERNAME_REQUIRED"] = "اسم المستخدم مطلوب.",
        ["AUTH_PASSWORD_REQUIRED"] = "كلمة المرور مطلوبة.",
        ["AUTH_EMAIL_REQUIRED"] = "البريد الإلكتروني غير صحيح.",
        ["AUTH_FULL_NAME_REQUIRED"] = "الاسم الكامل مطلوب.",
        ["AUTH_ACCOUNT_LOCKED"] = "تم قفل حسابك مؤقتًا. يرجى المحاولة مرة أخرى لاحقًا.",
        ["AUTH_USERNAME_TAKEN"] = "هذا الاسم مستخدم بالفعل.",
        ["AUTH_EMAIL_TAKEN"] = "يوجد حساب بالفعل بهذا البريد الإلكتروني.",
        ["AUTH_NATIONAL_ID_TAKEN"] = "يوجد حساب بالفعل بهذه الهوية.",
        ["AUTH_USERNAME_PATTERN"] = "يُسمح فقط بالأحرف والأرقام والنقاط والشرطات السفلية في اسم المستخدم.",
        ["AUTH_PASSWORD_MIN_LENGTH"] = "يجب أن تتكون كلمة المرور من 8 أحرف على الأقل.",
        ["AUTH_PASSWORD_UPPERCASE"] = "يجب أن تحتوي كلمة المرور على حرف كبير واحد على الأقل.",
        ["AUTH_PASSWORD_NUMBER"] = "يجب أن تحتوي كلمة المرور على رقم واحد على الأقل.",
        ["AUTH_VALIDATION_FAILED"] = "فشل التحقق.",
        ["AUTH_CONFIRM_PASSWORD_MISMATCH"] = "كلمتا المرور غير متطابقتين.",
        ["AUTH_NATIONAL_ID_FORMAT"] = "يجب أن يتكون رقم الهوية من 8 إلى 12 رقمًا.",
        ["AUTH_AUTHENTICATION_REQUIRED"] = "المصادقة مطلوبة.",
        ["AUTH_FORBIDDEN"] = "ممنوع.",
        ["AUTH_NOT_FOUND"] = "المورد غير موجود.",
        ["AUTH_CONFLICT"] = "تعارض.",
        ["AUTH_TOO_MANY_REQUESTS"] = "طلبات كثيرة جدًا.",
        ["AUTH_UNEXPECTED_ERROR"] = "حدث خطأ غير متوقع."
    };

    private static readonly Dictionary<string, string> Kurdish = new(StringComparer.Ordinal)
    {
        ["AUTH_LOGIN_SUCCESS"] = "چوونەژوورەوە سەرکەوتوو بوو.",
        ["AUTH_SIGNUP_SUCCESS"] = "تۆمارکردن سەرکەوتوو بوو.",
        ["AUTH_SESSION_REFRESHED"] = "دامەزراوەکە نوێکرایەوە.",
        ["AUTH_INVALID_CREDENTIALS"] = "ناوی بەکارھێنەر یان تێپەڕوشە نادروستە.",
        ["AUTH_USERNAME_REQUIRED"] = "ناوی بەکارھێنەر پێویستە.",
        ["AUTH_PASSWORD_REQUIRED"] = "تێپەڕوشە پێویستە.",
        ["AUTH_EMAIL_REQUIRED"] = "ئیمەیڵی دروست پێویستە.",
        ["AUTH_FULL_NAME_REQUIRED"] = "ناوی تەواو پێویستە.",
        ["AUTH_ACCOUNT_LOCKED"] = "ھەژمارەکەت تا کاتێکی دیاریکراو قوفڵ کراوە. تکایە دواتر دووبارە هەوڵ بدە.",
        ["AUTH_USERNAME_TAKEN"] = "ئەم ناوە پێشتر تۆمارکراوە.",
        ["AUTH_EMAIL_TAKEN"] = "ھەژمارێک هەیە بەم ئیمەیڵە.",
        ["AUTH_NATIONAL_ID_TAKEN"] = "ھەژمارێک هەیە بەم ناسنامەیە.",
        ["AUTH_USERNAME_PATTERN"] = "تەنها پیتی لاتین، ژمارە، خاڵ و هێڵی خوارەوە بەکار بھێنە.",
        ["AUTH_PASSWORD_MIN_LENGTH"] = "تێپەڕوشە دەبێت لانیکەم ٨ نووسە بێت.",
        ["AUTH_PASSWORD_UPPERCASE"] = "تێپەڕوشە دەبێت لانیکەم یەک پیتی گەورە لە خۆ بگرێت.",
        ["AUTH_PASSWORD_NUMBER"] = "تێپەڕوشە دەبێت لانیکەم یەک ژمارە لە خۆ بگرێت.",
        ["AUTH_VALIDATION_FAILED"] = "پشتڕاستکردنەوە سەرکەوتوو نەبوو.",
        ["AUTH_CONFIRM_PASSWORD_MISMATCH"] = "تێپەڕوشەکان یەک ناگرن.",
        ["AUTH_NATIONAL_ID_FORMAT"] = "ژمارەی ناسنامە دەبێت لە ٨ تا ١٢ ژمارە بێت.",
        ["AUTH_AUTHENTICATION_REQUIRED"] = "پەسەندکردن پێویستە.",
        ["AUTH_FORBIDDEN"] = "قەدەغەیە.",
        ["AUTH_NOT_FOUND"] = "سەرچاوەکە نەدۆزرایەوە.",
        ["AUTH_CONFLICT"] = "ڕکەوتن.",
        ["AUTH_TOO_MANY_REQUESTS"] = "داواکردنەکان زۆرە.",
        ["AUTH_UNEXPECTED_ERROR"] = "هەڵەیەکی چاوەڕوان نەکراو ڕووی دا." 
    };

    public static string Translate(string? key, string? locale)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            return string.Empty;
        }

        var normalizedLocale = NormalizeLocale(locale);
        var cacheKey = $"{normalizedLocale}:{key.Trim()}";
        return Cache.GetOrAdd(cacheKey, _ => BuildTranslation(key.Trim(), normalizedLocale));
    }

    public static bool TryTranslate(string? key, string? locale, out string message)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            message = string.Empty;
            return false;
        }

        message = Translate(key, locale);
        return !string.Equals(message, key, StringComparison.Ordinal);
    }

    public static string NormalizeLocale(string? locale)
    {
        if (string.IsNullOrWhiteSpace(locale))
        {
            return "en";
        }

        var candidate = locale.Trim();
        var firstPart = candidate.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)[0];
        var languageToken = firstPart.Split(';', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)[0];
        var normalized = languageToken.Split('-', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)[0];

        return normalized.ToLowerInvariant() switch
        {
            "ar" => "ar",
            "ku" => "ku",
            _ => "en"
        };
    }

    private static string BuildTranslation(string key, string locale)
    {
        var english = English.TryGetValue(key, out var englishValue) ? englishValue : key;

        return locale switch
        {
            "ar" => Arabic.TryGetValue(key, out var arabicValue) ? arabicValue : english,
            "ku" => Kurdish.TryGetValue(key, out var kurdishValue) ? kurdishValue : english,
            _ => english
        };
    }
}
