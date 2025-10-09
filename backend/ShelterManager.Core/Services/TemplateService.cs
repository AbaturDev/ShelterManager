using ShelterManager.Core.Services.Abstractions;

namespace ShelterManager.Core.Services;

public class TemplateService : ITemplateService
{
    public string LoadTemplate(string path, Dictionary<string, string> parameters)
    {
        var fullPath = Path.Combine(AppContext.BaseDirectory,path);
        
        if (!File.Exists(fullPath))
            throw new FileNotFoundException($"Template not found at path '{fullPath}'.");
        
        var content = File.ReadAllText(fullPath);
        
        foreach (var (key, value) in parameters)
        {
            content = content.Replace($"{{{{{key}}}}}", value);
        }

        return content;
    }
}