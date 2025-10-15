using DinkToPdf;
using DinkToPdf.Contracts;
using ShelterManager.Core.Services.Abstractions;

namespace ShelterManager.Core.Services;

public class PdfService : IPdfService
{
    private readonly IConverter _converter;
    
    public PdfService(IConverter converter)
    {
        _converter = converter;
    }
    
    public Stream GeneratePdfFromHtml(string htmlContent)
    {
        var document = new HtmlToPdfDocument()
        {
            GlobalSettings = {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4,
                Margins = new MarginSettings { Top = 10, Right = 10, Bottom = 10, Left = 10 },
            },
            Objects = {
                new ObjectSettings {
                    PagesCount = true,
                    HtmlContent = htmlContent,
                    WebSettings = { DefaultEncoding = "utf-8" },
                    FooterSettings = { FontSize = 9, Right = "[page]" },
                }
            }
        };

        var documentFile = _converter.Convert(document);
        
        return new MemoryStream(documentFile);
    }
}