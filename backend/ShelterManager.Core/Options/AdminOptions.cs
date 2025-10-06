using System.ComponentModel.DataAnnotations;

namespace ShelterManager.Core.Options;

public class AdminOptions
{
    public const string SectionName = "Admin";
    
    [Required] public required string Email { get; init; }
    [Required] public required string Password { get; init; }
}