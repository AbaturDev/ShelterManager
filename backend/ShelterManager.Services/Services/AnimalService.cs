using ShelterManager.Services.Services.Abstractions;

namespace ShelterManager.Services.Services;

public class AnimalService : IAnimalService
{
    public async Task<string> ListAnimalsAsync()
    {
        // throw new Exception("Aaa");
        return "abc";
    }
}