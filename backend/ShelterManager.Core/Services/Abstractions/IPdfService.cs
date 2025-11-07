namespace ShelterManager.Core.Services.Abstractions;

public interface IPdfService
{
    Task<Stream> GeneratePdfFromHtml(string htmlContent, CancellationToken ct = default);
}