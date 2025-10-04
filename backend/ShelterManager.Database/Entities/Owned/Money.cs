using Microsoft.EntityFrameworkCore;

namespace ShelterManager.Database.Entities.Owned;

[Owned]
public sealed record Money
{
    public decimal Amount { get; set; }
    public required string CurrencyCode { get; set; }
}