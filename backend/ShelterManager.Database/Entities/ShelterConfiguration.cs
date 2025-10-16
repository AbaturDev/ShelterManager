using ShelterManager.Database.Commons;

namespace ShelterManager.Database.Entities;

public sealed record ShelterConfiguration : BaseEntity
{
    public required string Name { get; init; }
    public required string Street { get; init; }
    public required string PostalCode { get; init; }
    public required string City { get; init; }
    public required string PhoneNumber { get; init; }
    public required string Email { get; init; }
}