namespace ShelterManager.Core.Utils;

public static class PasswordGenerator
{
    private const string Lowercase = "abcdefghijklmnopqrstuvwxyz";
    private const string Uppercase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private const string Digits = "0123456789";
    private const string Specials = "!@#$%^&*()";
    
    private static readonly string AllChars = Lowercase + Uppercase + Digits + Specials;

    public static string GeneratePassword(int length)
    {
        if (length < 4)
            throw new ArgumentException("Password length must be at least 4 to include all character types.");

        var random = new Random();
        var password = new List<char>
        {
            Lowercase[random.Next(Lowercase.Length)],
            Uppercase[random.Next(Uppercase.Length)],
            Digits[random.Next(Digits.Length)],
            Specials[random.Next(Specials.Length)]
        };

        for (int i = password.Count; i < length; i++)
        {
            password.Add(AllChars[random.Next(AllChars.Length)]);
        }

        return new string(password.OrderBy(_ => random.Next()).ToArray());
    }
}