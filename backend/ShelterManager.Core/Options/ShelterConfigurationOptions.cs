using System.ComponentModel.DataAnnotations;

namespace ShelterManager.Core.Options;

public class ShelterConfigurationOptions
{
    public const string SectionName = "ShelterConfiguration";
    
    [Required] public required string Name { get; init; }
    [Required] public required string Street { get; init; }
    [Required] public required string PostalCode { get; init; }
    [Required] public required string City { get; init; }
    [Required] public required string PhoneNumber { get; init; }
    [Required] public required string Email { get; init; }
}