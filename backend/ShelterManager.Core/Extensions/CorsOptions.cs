using System.ComponentModel.DataAnnotations;

namespace ShelterManager.Core.Extensions;

public class CorsOptions
{
    public const string SectionName = "Cors";
    
    [Required] public required string[] Headers { get; init; }
    [Required] public required string[] ExposedHeaders { get; init; }
    [Required] public required string[] Methods { get; init; }
    [Required] public required string[] Origins { get; init; }
    [Required] public required bool AllowCredentials { get; init; }    
}