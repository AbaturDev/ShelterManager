namespace ShelterManager.Services.Constants;

public static class RegisterEmailSubjects
{
    private static readonly Dictionary<string, string> LanguageSubjects = new()
    {
        { "pl", "Twoje konto zostało utworzone" },
        { "en", "Your account has been created" }
    };
    
    public static string GetSubject(string languageCode)
    {
        if (LanguageSubjects.TryGetValue(languageCode, out var subject))
            return subject;
        
        return LanguageSubjects["en"];
    }
}