using Microsoft.EntityFrameworkCore;
using ShelterManager.Database.Contexts;
using ShelterManager.Database.Enums;
using ShelterManager.Services.Dtos.SystemInfo;

namespace ShelterManager.Services.Features.SystemInfo;

public static class GetStatistics
{
    public static async Task<StatisticsDto> GetShelterStatisticsAsync(
        ShelterManagerContext context,
        CancellationToken ct
        )
    {
        var animalsCount = await context.Animals.CountAsync(ct);
        var currentAdoptions = await context.Adoptions
            .Where(x => x.Status == AdoptionStatus.Pending)
            .CountAsync(ct);
        var upComingEvents = await context.Events
            .Where(x => !x.IsDone)
            .CountAsync(ct);

        return new StatisticsDto
        {
            AnimalsCount = animalsCount,
            CurrentAdoptionProcessCount = currentAdoptions,
            UpcomingEventsCount = upComingEvents,
        };
    }
}