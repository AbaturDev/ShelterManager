namespace ShelterManager.Services.Dtos.Commons;

public sealed record FileStreamDto
{
    public required string FileExtension { get; init; }
    public required Stream Stream { get; set; }
}