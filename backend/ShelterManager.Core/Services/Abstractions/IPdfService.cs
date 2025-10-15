namespace ShelterManager.Core.Services.Abstractions;

public interface IPdfService
{
    Stream GeneratePdfFromHtml(string htmlContent);
}