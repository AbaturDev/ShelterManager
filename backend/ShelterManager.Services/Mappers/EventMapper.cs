using ShelterManager.Database.Entities;
using ShelterManager.Services.Dtos.Commons;
using ShelterManager.Services.Dtos.Events;

namespace ShelterManager.Services.Mappers;

public static class EventMapper
{
    public static EventDto MapToEventDto(Event eventEntity)
    {
        return new EventDto
        {
            Id = eventEntity.Id,
            CreatedAt = eventEntity.CreatedAt,
            UpdatedAt = eventEntity.UpdatedAt,
            Title = eventEntity.Title,
            Description = eventEntity.Description,
            Date = eventEntity.Date,
            IsDone = eventEntity.IsDone,
            CompletedAt = eventEntity.CompletedAt,
            Location = eventEntity.Location,
            AnimalId = eventEntity.AnimalId,
            UserId = eventEntity.UserId,
            CompletedByUserId = eventEntity.CompletedByUserId,
            Cost = MoneyMapper.MapToMoneyDto(eventEntity.Cost)
        };
    }
}