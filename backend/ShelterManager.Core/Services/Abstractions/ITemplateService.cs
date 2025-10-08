namespace ShelterManager.Core.Services.Abstractions;

public interface ITemplateService
{
    string LoadTemplate(string path, Dictionary<string, string> parameters);
}