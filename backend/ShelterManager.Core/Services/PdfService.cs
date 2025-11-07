using Microsoft.Playwright;
using ShelterManager.Core.Services.Abstractions;

namespace ShelterManager.Core.Services;

public class PdfService : IPdfService
{
    public async Task<Stream> GeneratePdfFromHtml(string htmlContent, CancellationToken ct)
    {
        using var playwright = await Playwright.CreateAsync();
        var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = true });
        
        var page = await browser.NewPageAsync();
        await page.SetContentAsync(htmlContent);

        var pdfFile = await page.PdfAsync();

        await page.CloseAsync();
        
        return new MemoryStream(pdfFile);
    }
}