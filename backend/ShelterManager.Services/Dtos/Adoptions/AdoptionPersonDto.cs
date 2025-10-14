namespace ShelterManager.Services.Dtos.Adoptions;

public sealed record AdoptionPersonDto
{
    public required string Name { get; init; }
    public required string Surname { get; init; }
    public required string PhoneNumber { get; init; }
    public required string Email { get; init; }
    public required string City { get; init; }
    public required string Street { get; init; }
    public required string PostalCode { get; init; }
}