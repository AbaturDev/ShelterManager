using ShelterManager.Services.Constants;

namespace ShelterManager.Api.Utils;

public static class LanguageUtils
{
    private const string DefaultLanguageCode = SupportedLanguages.English;

    public static string GetLanguageCode(string? language)
    {
        if (string.IsNullOrWhiteSpace(language))
        {
            return DefaultLanguageCode;
        }

        var langCode = language.ToLowerInvariant();
        return SupportedLanguages.AllSupportedLanguages.Contains(langCode) ? langCode : DefaultLanguageCode;
    }
}