using Microsoft.EntityFrameworkCore;

namespace ShelterManager.Database.Entities.Owned;

[Owned]
public sealed record AdoptionPerson
{
    public required string Name { get; init; }
    public required string Surname { get; init; }
    public required string PhoneNumber { get; init; }
    public required string Email { get; init; }
    public required string Pesel { get; init; }
    public required string DocumentId { get; init; }
    public required string City { get; init; }
    public required string Street { get; init; }
    public required string PostalCode { get; init; }
}