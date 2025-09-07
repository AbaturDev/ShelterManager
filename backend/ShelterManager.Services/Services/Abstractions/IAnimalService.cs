namespace ShelterManager.Services.Services.Abstractions;

public interface IAnimalService
{
    Task<string> ListAnimalsAsync();
}