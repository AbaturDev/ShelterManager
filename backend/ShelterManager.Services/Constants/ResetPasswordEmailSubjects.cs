namespace ShelterManager.Services.Constants;

public class ResetPasswordEmailSubjects
{
    private static readonly Dictionary<string, string> LanguageSubjects = new()
    {
        { "pl", "Resetowanie hasła" },
        { "en", "Password Reset" }
    };
    
    public static string GetSubject(string languageCode)
    {
        if (LanguageSubjects.TryGetValue(languageCode, out var subject))
            return subject;
        
        return LanguageSubjects["en"];
    }
}