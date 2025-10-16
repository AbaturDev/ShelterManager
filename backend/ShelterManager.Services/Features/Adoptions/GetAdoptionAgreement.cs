using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ShelterManager.Core.Exceptions;
using ShelterManager.Core.Services.Abstractions;
using ShelterManager.Database.Contexts;
using ShelterManager.Database.Entities;
using ShelterManager.Database.Enums;
using ShelterManager.Services.Constants;
using ShelterManager.Services.Dtos.Commons;

namespace ShelterManager.Services.Features.Adoptions;

public static class GetAdoptionAgreement
{
    private const string PdfFileExtension = ".pdf";
    
    public static async Task<FileStreamDto> GetAdoptionAgreementAsync(
        Guid adoptionId,
        string lang,
        ShelterManagerContext context,
        IPdfService pdfService,
        ITemplateService templateService,
        TimeProvider timeProvider,
        IUserContextService userContext,
        UserManager<User> userManager,
        CancellationToken ct
        )
    {
        var adoption = await context.Adoptions
            .Include(x => x.Animal)
            .ThenInclude(a => a.Breed)
            .ThenInclude(b => b.Species)
            .FirstOrDefaultAsync(x => x.Id == adoptionId, ct);

        if (adoption is null)
        {
            throw new NotFoundException("Adoption not found");
        }

        var userId = userContext.GetCurrentUserId();
        var user = await userManager.Users
            .FirstAsync(x => x.Id == userId, ct);
        
        var shelterConfiguration = await context.ShelterConfigurations
            .FirstAsync(ct);
        
        var date = DateOnly.FromDateTime(timeProvider.GetUtcNow().UtcDateTime);
        
        var templateParameters = new Dictionary<string, string>
        {
            {"ShelterName", shelterConfiguration.Name},
            {"ShelterStreet", shelterConfiguration.Street},
            {"ShelterPostalCode", shelterConfiguration.PostalCode},
            {"ShelterCity", shelterConfiguration.City},
            {"ShelterPhoneNumber", shelterConfiguration.PhoneNumber},
            {"ShelterEmail", shelterConfiguration.Email},
            {"AdoptionStartDate", date.ToString()},
            {"UserName", user.Name},
            {"UserSurname", user.Surname},
            {"PersonName", adoption.Person.Name},
            {"PersonSurname", adoption.Person.Surname},
            {"PersonDocumentId", adoption.Person.DocumentId},
            {"PersonPesel", adoption.Person.Pesel},
            {"PersonStreet", adoption.Person.Street},
            {"PersonPostalCode", adoption.Person.PostalCode},
            {"PersonCity", adoption.Person.City},
            {"PersonPhoneNumber", adoption.Person.PhoneNumber},
            {"PersonEmail", adoption.Person.Email},
            {"AnimalName", adoption.Animal.Name},
            {"AnimalSpecies", adoption.Animal.Breed.Species.Name},
            {"AnimalBreed", adoption.Animal.Breed.Name},
            {"AnimalSex", SexLanguageTranslation(adoption.Animal.Sex, lang)},
            {"AnimalAge", adoption.Animal.Age.ToString() ?? UnknownAgeByLanguage(lang)},
            {"AnimalId", adoption.Animal.Id.ToString()},
        };
        var content = templateService.LoadTemplate($"Templates\\Adoptions\\{lang}\\AdoptionAgreement.html", templateParameters);

        var pdfStream = pdfService.GeneratePdfFromHtml(content);
        
        return new FileStreamDto
        {
            Stream = pdfStream,
            FileExtension = PdfFileExtension
        };
    }

    private static string UnknownAgeByLanguage(string lang)
    {
        return lang switch
        {
            SupportedLanguages.Polish  => "Wiek nieznany",
            SupportedLanguages.English => "Unknown age",
            _ => "Unknown age"
        };
    }

    private static string SexLanguageTranslation(Sex sex, string lang)
    {
        if (lang == SupportedLanguages.English)
        {
            return sex.ToString();
        }

        return sex switch
        {
            Sex.Male when lang == SupportedLanguages.Polish => "Samiec",
            Sex.Female when lang == SupportedLanguages.Polish => "Samica",
            _ => throw new ArgumentOutOfRangeException(nameof(sex), sex, null)
        };
    }
}