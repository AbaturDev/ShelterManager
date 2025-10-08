using System.ComponentModel.DataAnnotations;

namespace ShelterManager.Core.Options;

public class EmailOptions
{
    public const string SectionName = "Email";

    [Required] public required string Host { get; init; }
    [Required] public int Port { get; init; }
    [Required] public required string UserName { get; init; }
    [Required] public required string Password { get; init; }
    [Required] public required EmailOptionsFromSection From { get; init; }
}

public class EmailOptionsFromSection
{
    [Required] public required string Name { get; init; }
    [Required] public required string Email { get; init; }
}
