using System.ComponentModel.DataAnnotations;

namespace ShelterManager.Core.Options;

public class FrontendOptions
{
    public const string SectionName = "Frontend";
    
    [Required] public required string Url { get; init; }
}