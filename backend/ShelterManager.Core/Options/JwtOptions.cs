using System.ComponentModel.DataAnnotations;

namespace ShelterManager.Core.Options;

public class JwtOptions
{
    public const string SectionName = "Jwt";
    
    [Required] public required string Secret { get; init; }
    [Required] public required string Audience { get; init; }
    [Required] public required string Issuer { get; init; }
}