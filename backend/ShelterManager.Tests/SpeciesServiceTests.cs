using Microsoft.EntityFrameworkCore;
using ShelterManager.Core.Exceptions;
using ShelterManager.Database.Contexts;
using ShelterManager.Database.Entities;
using ShelterManager.Services.Dtos.Commons;
using ShelterManager.Services.Dtos.Species;
using ShelterManager.Services.Services;

namespace ShelterManager.Tests;

public class SpeciesServiceTests
{
    [Fact]
    public async Task CreateSpeciesAsync_ShouldCreateSpecies()
    {
        // Arrange
        var optionsBuilder = new DbContextOptionsBuilder<ShelterManagerContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString());

        var timeProvider = TimeProvider.System;
        await using var context = new ShelterManagerContext(optionsBuilder.Options, timeProvider);
            
        var speciesService = new SpeciesService(context);

        var createDto = new CreateSpeciesDto
        {
            Name = "Dog"
        };

        // Act
        var newSpeciesId = await speciesService.CreateSpeciesAsync(createDto, CancellationToken.None);

        // Assert
        Assert.NotEqual(Guid.Empty, newSpeciesId);
            
        var addedSpecies = await context.Species.FirstOrDefaultAsync(s => s.Id == newSpeciesId);
        Assert.NotNull(addedSpecies);
    }
    
    [Fact]
    public async Task CreateSpeciesAsync_ShouldThrowBadRequest()
    {
        // Arrange
        var optionsBuilder = new DbContextOptionsBuilder<ShelterManagerContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString());

        var timeProvider = TimeProvider.System;
        await using var context = new ShelterManagerContext(optionsBuilder.Options, timeProvider);

        const string speciesName = "Dog";
        context.Species.Add(new Species { Name = speciesName });
        await context.SaveChangesAsync();
        
        var speciesService = new SpeciesService(context);

        var createDto = new CreateSpeciesDto
        {
            Name = speciesName
        };

        // Act & Assert
        await Assert.ThrowsAsync<BadRequestException>(() => speciesService.CreateSpeciesAsync(createDto, CancellationToken.None));
    }
    
    [Fact]
    public async Task DeleteSpeciesAsync_ShouldDeleteSpecies()
    {
        // Arrange
        var optionsBuilder = new DbContextOptionsBuilder<ShelterManagerContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString());

        var timeProvider = TimeProvider.System;
        await using var context = new ShelterManagerContext(optionsBuilder.Options, timeProvider);
        
        var species = new Species { Id = Guid.NewGuid(), Name = "Dog" };
        context.Species.Add(species);
        await context.SaveChangesAsync();
            
        var speciesService = new SpeciesService(context);
            
        // Act
        await speciesService.DeleteSpeciesAsync(species.Id, CancellationToken.None);
            
        // Assert
        var result = await context.Species.FirstOrDefaultAsync(s => s.Id == species.Id);
        Assert.Null(result);
    }
        
    [Fact]
    public async Task DeleteSpeciesAsync_ShouldThrowNotFound()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ShelterManagerContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var timeProvider = TimeProvider.System;
        await using var context = new ShelterManagerContext(options, timeProvider);

        var speciesService = new SpeciesService(context);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() =>
            speciesService.DeleteSpeciesAsync(Guid.NewGuid(), CancellationToken.None));
    }
    
    [Fact]
    public async Task GetSpeciesByIdAsync_ShouldThrowNotFound()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ShelterManagerContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var timeProvider = TimeProvider.System;
        await using var context = new ShelterManagerContext(options, timeProvider);

        var speciesService = new SpeciesService(context);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() =>
            speciesService.GetSpeciesByIdAsync(Guid.NewGuid(), CancellationToken.None));
    }
    
    [Fact]
    public async Task GetSpeciesByIdAsync_ShouldReturnSpecies()
    {
        // Arrange
        var optionsBuilder = new DbContextOptionsBuilder<ShelterManagerContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString());

        var timeProvider = TimeProvider.System;
        await using var context = new ShelterManagerContext(optionsBuilder.Options, timeProvider);
        
        var species = new Species { Id = Guid.NewGuid(), Name = "Dog" };
        context.Species.Add(species);
        await context.SaveChangesAsync();
            
        var speciesService = new SpeciesService(context);
            
        // Act
        var result = await speciesService.GetSpeciesByIdAsync(species.Id, CancellationToken.None);
            
        // Assert
        Assert.NotNull(result);
        Assert.Equal(species.Id, result.Id);
        Assert.Equal(species.Name, result.Name);
    }
    
    [Fact]
    public async Task ListSpeciesAsync_ShouldReturnPaginatedSpecies()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ShelterManagerContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var timeProvider = TimeProvider.System;
        await using var context = new ShelterManagerContext(options, timeProvider);

        for (var i = 1; i <= 13; i++)
        {
            context.Species.Add(new Species
            {
                Id = Guid.NewGuid(),
                Name = $"Species {i}",
            });
        }
        await context.SaveChangesAsync();

        var speciesService = new SpeciesService(context);

        var pageQueryFilter = new PageQueryFilter();
        
        // Act
        var result = await speciesService.ListSpeciesAsync(pageQueryFilter, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(13, result.TotalItemsCount);
        Assert.Equal(pageQueryFilter.Page, result.Page);
        Assert.Equal(pageQueryFilter.PageSize, result.PageSize);
        Assert.All(result.Items, dto => Assert.NotNull(dto.Name));
    }
}