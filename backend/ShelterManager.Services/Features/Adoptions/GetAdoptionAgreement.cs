using ShelterManager.Core.Services.Abstractions;
using ShelterManager.Database.Contexts;

namespace ShelterManager.Services.Features.Adoptions;

public static class GetAdoptionAgreement
{
    public static async Task<Stream> GetAdoptionAgreementAsync(
        Guid adoptionId,
        ShelterManagerContext context,
        IPdfService pdfService,
        ITemplateService templateService,
        CancellationToken ct
        )
    {

        return new MemoryStream();
    }
}