namespace ShelterManager.Api.Utils;

public static class LanguageUtils
{
    private const string DefaultLanguageCode = "en";
    private static readonly HashSet<string> SupportedLanguages = ["en", "pl"];

    public static string GetLanguageCode(string? language)
    {
        if (string.IsNullOrWhiteSpace(language))
        {
            return DefaultLanguageCode;
        }

        var langCode = language.ToLowerInvariant();
        return SupportedLanguages.Contains(langCode) ? langCode : DefaultLanguageCode;
    }
}