using Microsoft.Extensions.Logging;
using ShelterManager.Common.Exceptions;
using ShelterManager.Services.Services.Abstractions;

namespace ShelterManager.Services.Services;

public class AnimalService : IAnimalService
{
    private readonly ILogger<AnimalService> _logger;
    
    public AnimalService(ILogger<AnimalService> logger)
    {
        _logger = logger;
    }
    public async Task<string> ListAnimalsAsync()
    {
        _logger.LogInformation("AAAsfasfasf asfasfasfasf asfasf");
        
        throw new NotFoundException("Aaa");
        // return "abc";
    }
}