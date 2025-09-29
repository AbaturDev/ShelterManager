using System.ComponentModel.DataAnnotations;

namespace ShelterManager.Core.Options;

public class ApiOptions
{
    public const string SectionName = "Api";

    [Required] public required int Version { get; init; }
    [Required] public required string Title { get; init; }
    [Required] public required string Description { get; init; }
}