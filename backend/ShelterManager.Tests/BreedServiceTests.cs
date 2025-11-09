using Microsoft.EntityFrameworkCore;
using ShelterManager.Core.Exceptions;
using ShelterManager.Database.Contexts;
using ShelterManager.Database.Entities;
using ShelterManager.Services.Dtos.Breeds;
using ShelterManager.Services.Dtos.Commons;
using ShelterManager.Services.Services;

namespace ShelterManager.Tests;

public class BreedServiceTests
{
    [Fact]
    public async Task CreateBreedAsync_ShouldCreateBreed()
    {
        // Arrange
        var optionsBuilder = new DbContextOptionsBuilder<ShelterManagerContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString());

        var timeProvider = TimeProvider.System;
        await using var context = new ShelterManagerContext(optionsBuilder.Options, timeProvider);

        var species = new Species { Id = Guid.NewGuid(), Name = "Dog" };
        context.Species.Add(species);
        await context.SaveChangesAsync();
        
        var breedService = new BreedService(context);

        var breedDto = new CreateBreedDto
        {
            Name = "Labrador"
        };

        // Act
        var newBreedId = await breedService.CreateBreedAsync(breedDto, species.Id, CancellationToken.None);

        // Assert
        Assert.NotEqual(Guid.Empty, newBreedId);
            
        var addedBreed = await context.Breeds.FirstOrDefaultAsync(b => b.Id == newBreedId);
        Assert.NotNull(addedBreed);
    }
    
    [Fact]
    public async Task CreateBreedAsync_ShouldThrowNotFound()
    {
        // Arrange
        var optionsBuilder = new DbContextOptionsBuilder<ShelterManagerContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString());

        var timeProvider = TimeProvider.System;
        await using var context = new ShelterManagerContext(optionsBuilder.Options, timeProvider);
        
        var breedService = new BreedService(context);

        var breedDto = new CreateBreedDto
        {
            Name = "Labrador"
        };

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() =>
            breedService.CreateBreedAsync(breedDto, Guid.NewGuid(), CancellationToken.None));
    }
    
    [Fact]
    public async Task CreateBreedAsync_ShouldThrowBadRequest()
    {
        // Arrange
        var optionsBuilder = new DbContextOptionsBuilder<ShelterManagerContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString());

        var timeProvider = TimeProvider.System;
        await using var context = new ShelterManagerContext(optionsBuilder.Options, timeProvider);

        var species = new Species { Id = Guid.NewGuid(), Name = "Dog" };
        context.Species.Add(species);
        context.Breeds.Add(new Breed { Id = Guid.NewGuid(), Name = "Labrador", SpeciesId = species.Id });
        await context.SaveChangesAsync();
        
        var breedService = new BreedService(context);

        var breedDto = new CreateBreedDto
        {
            Name = "Labrador"
        };

        // Act & Assert
        await Assert.ThrowsAsync<BadRequestException>(() =>
            breedService.CreateBreedAsync(breedDto, species.Id, CancellationToken.None));
    }
    
    [Fact]
    public async Task DeleteBreedAsync_ShouldDeleteBreed()
    {
        // Arrange
        var optionsBuilder = new DbContextOptionsBuilder<ShelterManagerContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString());
    
        var timeProvider = TimeProvider.System;
        await using var context = new ShelterManagerContext(optionsBuilder.Options, timeProvider);
        
        var species = new Species { Id = Guid.NewGuid(), Name = "Dog" };
        context.Species.Add(species);
        var breed = new Breed { Id = Guid.NewGuid(), Name = "Labrador", SpeciesId = species.Id };
        context.Breeds.Add(breed);
        await context.SaveChangesAsync();
            
        var breedService = new BreedService(context);
            
        // Act
        await breedService.DeleteBreedAsync(breed.Id, species.Id, CancellationToken.None);
            
        // Assert
        var result = await context.Breeds.FirstOrDefaultAsync(b => b.Id == breed.Id);
        Assert.Null(result);
    }
    
    [Fact]
    public async Task DeleteBreedAsync_ShouldThrowNotFound()
    {
        // Arrange
        var optionsBuilder = new DbContextOptionsBuilder<ShelterManagerContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString());
    
        var timeProvider = TimeProvider.System;
        await using var context = new ShelterManagerContext(optionsBuilder.Options, timeProvider);
        
        var species = new Species { Id = Guid.NewGuid(), Name = "Dog" };
        context.Species.Add(species);
        var breed = new Breed { Id = Guid.NewGuid(), Name = "Labrador", SpeciesId = species.Id };
        context.Breeds.Add(breed);
        await context.SaveChangesAsync();
            
        var breedService = new BreedService(context);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() =>
            breedService.DeleteBreedAsync(breed.Id, Guid.NewGuid(), CancellationToken.None));
        await Assert.ThrowsAsync<NotFoundException>(() =>
            breedService.DeleteBreedAsync(Guid.NewGuid(), species.Id, CancellationToken.None));
        await Assert.ThrowsAsync<NotFoundException>(() =>
            breedService.DeleteBreedAsync(Guid.NewGuid(), Guid.NewGuid(), CancellationToken.None));
    }
    
    [Fact]
    public async Task GetBreedByIdAsync_ShouldReturnBreed()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ShelterManagerContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var timeProvider = TimeProvider.System;
        await using var context = new ShelterManagerContext(options, timeProvider);

        var species = new Species { Id = Guid.NewGuid(), Name = "Dog" };
        context.Species.Add(species);
        var breed = new Breed { Id = Guid.NewGuid(), Name = "Labrador", SpeciesId = species.Id };
        context.Breeds.Add(breed);
        await context.SaveChangesAsync();

        var service = new BreedService(context);

        // Act
        var result = await service.GetBreedByIdAsync(breed.Id, species.Id, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(breed.Id, result.Id);
        Assert.Equal(breed.Name, result.Name);
        Assert.Equal(species.Id, result.SpeciesId);
    }

    [Fact]
    public async Task GetBreedByIdAsync_ShouldThrowNotFound()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ShelterManagerContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var timeProvider = TimeProvider.System;
        await using var context = new ShelterManagerContext(options, timeProvider);

        var species = new Species { Id = Guid.NewGuid(), Name = "Dog" };
        context.Species.Add(species);
        var breed = new Breed { Id = Guid.NewGuid(), Name = "Labrador", SpeciesId = species.Id };
        context.Breeds.Add(breed);
        await context.SaveChangesAsync();

        var service = new BreedService(context);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() =>
            service.GetBreedByIdAsync(breed.Id, Guid.NewGuid(), CancellationToken.None));
        await Assert.ThrowsAsync<NotFoundException>(() =>
            service.GetBreedByIdAsync(Guid.NewGuid(), species.Id, CancellationToken.None));
        await Assert.ThrowsAsync<NotFoundException>(() =>
            service.GetBreedByIdAsync(Guid.NewGuid(), Guid.NewGuid(), CancellationToken.None));
    }

   [Fact]
    public async Task ListBreedsAsync_ShouldReturnPaginatedBreeds()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ShelterManagerContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var timeProvider = TimeProvider.System;
        await using var context = new ShelterManagerContext(options, timeProvider);

        var species = new Species { Id = Guid.NewGuid(), Name = "Dog" };
        context.Species.Add(species);

        for (var i = 1; i <= 15; i++)
        {
            context.Breeds.Add(new Breed { Id = Guid.NewGuid(), Name = $"Breed {i}", SpeciesId = species.Id });
        }
        await context.SaveChangesAsync();

        var service = new BreedService(context);
        var filter = new PageQueryFilter();

        // Act
        var result = await service.ListBreedsAsync(filter, species.Id, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(15, result.TotalItemsCount);
        Assert.Equal(10, result.Items.Count);
        Assert.Equal(1, result.Page);
        Assert.Equal(10, result.PageSize);
        Assert.All(result.Items, dto => Assert.NotNull(dto.Name));
    }

}