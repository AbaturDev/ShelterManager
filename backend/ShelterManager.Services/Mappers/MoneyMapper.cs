using ShelterManager.Database.Entities.Owned;
using ShelterManager.Services.Dtos.Commons;

namespace ShelterManager.Services.Mappers;

public static class MoneyMapper
{
    public static MoneyDto? MapToMoneyDto(Money? money)
    {
        if (money is null)
        {
            return null;
        }

        return new MoneyDto
        {
            Amount = money.Amount,
            CurrencyCode = money.CurrencyCode
        };
    }

    public static Money? MapToMoneyEntity(MoneyDto? moneyDto)
    {
        if (moneyDto is null)
        {
            return null;
        }

        return new Money
        {
            Amount = moneyDto.Amount,
            CurrencyCode = moneyDto.CurrencyCode
        };
    }
    
}