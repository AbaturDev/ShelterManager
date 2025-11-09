using Microsoft.EntityFrameworkCore;
using ShelterManager.Core.Exceptions;
using ShelterManager.Database.Contexts;
using ShelterManager.Database.Entities;
using ShelterManager.Database.Enums;
using ShelterManager.Services.Dtos.Animals;
using ShelterManager.Services.Services;

namespace ShelterManager.Tests;

public class AnimalServiceTests
{
    [Fact]
    public async Task CreateAnimalAsync_ShouldCreateAnimal()
    {
        // Arrange
        var optionsBuilder = new DbContextOptionsBuilder<ShelterManagerContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString());

        var timeProvider = TimeProvider.System;
        await using var context = new ShelterManagerContext(optionsBuilder.Options, timeProvider);

        var breed = new Breed { Id = Guid.NewGuid(), Name = "German Shepherd", SpeciesId = Guid.NewGuid() };
        context.Breeds.Add(breed);
        await context.SaveChangesAsync();
            
        var animalService = new AnimalService(context);
            
        var createDto = new CreateAnimalDto
        {
            Name = "Radzio",
            Age = 3,
            AdmissionDate = timeProvider.GetUtcNow(),
            Sex = Sex.Male,
            Description = "Friendly dog",
            BreedId = breed.Id
        };

        // Act
        var newAnimalId = await animalService.CreateAnimalAsync(createDto, CancellationToken.None);

        // Assert
        Assert.NotEqual(Guid.Empty, newAnimalId);
            
        var addedAnimal = await context.Animals.FirstOrDefaultAsync(a => a.Id == newAnimalId);
        Assert.NotNull(addedAnimal);
    }
        
    [Fact]
    public async Task CreateAnimalAsync_ShouldThrowBadRequest()
    {
        // Arrange
        var optionsBuilder = new DbContextOptionsBuilder<ShelterManagerContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString());

        var timeProvider = TimeProvider.System;
        await using var context = new ShelterManagerContext(optionsBuilder.Options, timeProvider);

        var animalService = new AnimalService(context);
            
        var createDto = new CreateAnimalDto
        {
            Name = "Radzio",
            Age = 3,
            AdmissionDate = timeProvider.GetUtcNow(),
            Sex = Sex.Male,
            Description = "Friendly dog",
            BreedId = Guid.NewGuid()
        };
            
        // Act & Assert
        await Assert.ThrowsAsync<BadRequestException>(() => animalService.CreateAnimalAsync(createDto, CancellationToken.None));
    }
        
    [Fact]
    public async Task GetAnimalAsync_ShouldReturnAnimal()
    {
        // Arrange
        var optionsBuilder = new DbContextOptionsBuilder<ShelterManagerContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString());

        var timeProvider = TimeProvider.System;
        await using var context = new ShelterManagerContext(optionsBuilder.Options, timeProvider);

        var speciesId = Guid.NewGuid();
        context.Species.Add(new Species { Id = speciesId, Name = "Dog" });
            
        var breedId = Guid.NewGuid();
        context.Breeds.Add(new Breed { Id = breedId, Name = "German Shepherd", SpeciesId = speciesId });
            
        var animal = new Animal
        {
            Id=Guid.NewGuid(), 
            AdmissionDate = timeProvider.GetUtcNow(),
            Status = AnimalStatus.InShelter,
            Name = "Radzio", 
            Age = 3,
            Sex = Sex.Male,
            Description = "Friendly dog",
            BreedId = breedId
        };
        context.Animals.Add(animal);
        await context.SaveChangesAsync();
            
        var animalService = new AnimalService(context);
            
        // Act
        var result = await animalService.GetAnimalByIdAsync(animal.Id, CancellationToken.None);
            
        // Assert
        Assert.NotNull(result);
        Assert.Equal(result.Id, animal.Id);
        Assert.Equal(result.Name, animal.Name);
    }
        
    [Fact]
    public async Task GetAnimalByIdAsync_ShouldThrowNotFound()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ShelterManagerContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var timeProvider = TimeProvider.System;
        await using var context = new ShelterManagerContext(options, timeProvider);

        var animalService = new AnimalService(context);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() =>
            animalService.GetAnimalByIdAsync(Guid.NewGuid(), CancellationToken.None));
    }
        
    [Fact]
    public async Task DeleteAnimalAsync_ShouldDeleteAnimal()
    {
        // Arrange
        var optionsBuilder = new DbContextOptionsBuilder<ShelterManagerContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString());

        var timeProvider = TimeProvider.System;
        await using var context = new ShelterManagerContext(optionsBuilder.Options, timeProvider);
            
        var animal = new Animal
        {
            Id = Guid.NewGuid(), 
            AdmissionDate = timeProvider.GetUtcNow(),
            Status = AnimalStatus.InShelter,
            Name = "Radzio", 
            Age = 3,
            Sex = Sex.Male,
            Description = "Friendly dog",
            BreedId = Guid.NewGuid()
        };
        context.Animals.Add(animal);
        await context.SaveChangesAsync();
            
        var animalService = new AnimalService(context);
            
        // Act
        await animalService.DeleteAnimalAsync(animal.Id, CancellationToken.None);
            
        // Assert
        var result = await context.Animals.FirstOrDefaultAsync(a => a.Id == animal.Id);
        Assert.Null(result);
    }
        
    [Fact]
    public async Task DeleteAnimalAsync_ShouldThrowNotFound()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ShelterManagerContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var timeProvider = TimeProvider.System;
        await using var context = new ShelterManagerContext(options, timeProvider);

        var animalService = new AnimalService(context);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() =>
            animalService.DeleteAnimalAsync(Guid.NewGuid(), CancellationToken.None));
    }

    [Fact]
    public async Task UpdateAnimalAsync_ShouldUpdateAnimal()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ShelterManagerContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var timeProvider = TimeProvider.System;
        await using var context = new ShelterManagerContext(options, timeProvider);

        var animal = new Animal
        {
            Id = Guid.NewGuid(),
            Name = "Radzio",
            Age = 3,
            AdmissionDate = timeProvider.GetUtcNow(),
            Sex = Sex.Male,
            Status = AnimalStatus.InShelter,
            Description = "Friendly dog",
            BreedId = Guid.NewGuid()
        };
        context.Animals.Add(animal);
        await context.SaveChangesAsync();

        var animalService = new AnimalService(context);

        var updateDto = new UpdateAnimalDto
        {
            Name = "Rex",
            Age = 4,
            AdmissionDate = timeProvider.GetUtcNow(),
            Status = AnimalStatus.Adopted,
            Description = "Updated description"
        };

        // Act
        await animalService.UpdateAnimalAsync(animal.Id, updateDto, CancellationToken.None);

        // Assert
        var updatedAnimal = await context.Animals.FirstOrDefaultAsync(a => a.Id == animal.Id);
        Assert.NotNull(updatedAnimal);
        Assert.Equal(updatedAnimal.Name, updateDto.Name);
        Assert.Equal(updatedAnimal.Age, updateDto.Age);
        Assert.Equal(updatedAnimal.Status, updateDto.Status);
        Assert.Equal(updatedAnimal.Description,updateDto.Description);
    }

    [Fact]
    public async Task UpdateAnimalAsync_ShouldThrowNotFound()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ShelterManagerContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var timeProvider = TimeProvider.System;
        await using var context = new ShelterManagerContext(options, timeProvider);

        var animalService = new AnimalService(context);

        var updateDto = new UpdateAnimalDto
        {
            Name = "Rex",
            Age = 4,
            AdmissionDate = timeProvider.GetUtcNow(),
            Status = AnimalStatus.Adopted,
            Description = "Updated description"
        };

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() =>
            animalService.UpdateAnimalAsync(Guid.NewGuid(), updateDto, CancellationToken.None));
    }

    [Fact]
    public async Task ListAnimalsAsync_ShouldReturnPaginatedAnimals()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ShelterManagerContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var timeProvider = TimeProvider.System;
        await using var context = new ShelterManagerContext(options, timeProvider);

        var breedId = Guid.NewGuid();
        context.Breeds.Add(new Breed { Id = breedId, Name = "German Shepherd", SpeciesId = Guid.NewGuid() });

        for (var i = 1; i <= 13; i++)
        {
            context.Animals.Add(new Animal
            {
                Id = Guid.NewGuid(),
                Name = $"Animal {i}",
                Age = i,
                AdmissionDate = timeProvider.GetUtcNow(),
                Sex = i % 2 == 0 ? Sex.Male : Sex.Female,
                Status = AnimalStatus.InShelter,
                Description = $"Description {i}",
                BreedId = breedId
            });
        }
        await context.SaveChangesAsync();

        var animalService = new AnimalService(context);

        var pageQueryFilter = new AnimalPageQueryFilter(Name: null, Sex: null, Status: null, Page: 1, PageSize: 10);

        // Act
        var result = await animalService.ListAnimalsAsync(pageQueryFilter, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(13, result.TotalItemsCount);
        Assert.Equal(1, result.Page);
        Assert.Equal(10, result.PageSize);
        Assert.All(result.Items, dto => Assert.NotNull(dto.Name));
    }
}