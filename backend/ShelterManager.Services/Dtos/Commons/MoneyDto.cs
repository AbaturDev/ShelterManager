namespace ShelterManager.Services.Dtos.Commons;

public sealed record MoneyDto
{
    public decimal Amount { get; init; }
    public required string CurrencyCode { get; init; }
}