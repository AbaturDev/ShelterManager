using Microsoft.EntityFrameworkCore;
using ShelterManager.Core.Services.Abstractions;
using ShelterManager.Database.Contexts;
using ShelterManager.Database.Entities;
using ShelterManager.Database.Enums;
using ShelterManager.Services.Dtos.Adoptions;
using ShelterManager.Services.Services;
using Moq;
using ShelterManager.Core.Exceptions;
using ShelterManager.Database.Entities.Owned;

namespace ShelterManager.Tests;

public class AdoptionServiceTests
{
    [Fact]
    public async Task CreateAdoptionAsync_ShouldCreateAdoption()
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

        var userContext = new Mock<IUserContextService>();
        var service = new AdoptionService(context, timeProvider, userContext.Object);

        var dto = new CreateAdoptionDto
        {
            AnimalId = animal.Id,
            Note = "Test note",
            Person = new AdoptionPersonDto
            {
                Name = "John",
                Surname = "Doe",
                DocumentId = "AB123456",
                Pesel = "12345678901",
                Email = "john@doe.com",
                PhoneNumber = "123456789",
                City = "Warsaw",
                Street = "Main Street",
                PostalCode = "00-001"
            }
        };

        // Act
        var id = await service.CreateAdoptionAsync(dto, CancellationToken.None);

        // Assert
        Assert.NotEqual(Guid.Empty, id);

        var created = await context.Adoptions
            .Include(x => x.Animal)
            .FirstOrDefaultAsync(x => x.Id == id);

        Assert.NotNull(created);
        Assert.Equal(animal.Id, created.AnimalId);
        Assert.Equal("Test note", created.Note);
        Assert.Equal(AdoptionStatus.Active, created.Status);
        Assert.NotNull(created.Person);
        Assert.Equal("John", created.Person.Name);
        Assert.NotEqual(default, created.StartAdoptionProcess);
    }

    [Fact]
    public async Task CreateAdoptionAsync_ShouldThrowBadRequest_WhenAnimalDoesNotExist()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ShelterManagerContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
    
        var timeProvider = TimeProvider.System;
        await using var context = new ShelterManagerContext(options, timeProvider);
        var userContext = new Mock<IUserContextService>();
        var service = new AdoptionService(context, timeProvider, userContext.Object);
    
        var dto = new CreateAdoptionDto
        {
            AnimalId = Guid.NewGuid(),
            Note = "Test note",
            Person = new AdoptionPersonDto
            {
                Name = "John",
                Surname = "Doe",
                DocumentId = "AB123456",
                Pesel = "12345678901",
                Email = "john@doe.com",
                PhoneNumber = "123456789",
                City = "Warsaw",
                Street = "Main Street",
                PostalCode = "00-001"
            }
        };
    
        // Act & Assert
        await Assert.ThrowsAsync<BadRequestException>(() =>
            service.CreateAdoptionAsync(dto, CancellationToken.None));
    }
    
    [Fact]
    public async Task CreateAdoptionAsync_ShouldThrowBadRequest_WhenAnimalHasActiveAdoption()
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
        
        context.Adoptions.Add(new Adoption
        {
            Id = Guid.NewGuid(),
            AnimalId = animal.Id,
            Status = AdoptionStatus.Active,
            StartAdoptionProcess = timeProvider.GetUtcNow(),
            Person = new AdoptionPerson
            {
                Name = "Old", 
                Surname = "Adopter",
                DocumentId = "AB123456",
                Pesel = "12345678901",
                Email = "old@adopter.com",
                PhoneNumber = "123456789",
                City = "Warsaw",
                Street = "Main Street",
                PostalCode = "00-001"
            }
        });
        
        await context.SaveChangesAsync();
    
        var userContext = new Mock<IUserContextService>();
        var service = new AdoptionService(context, timeProvider, userContext.Object);
    
        var dto = new CreateAdoptionDto
        {
            AnimalId = Guid.NewGuid(),
            Note = "Test note",
            Person = new AdoptionPersonDto
            {
                Name = "John",
                Surname = "Doe",
                DocumentId = "AB123456",
                Pesel = "12345678901",
                Email = "john@doe.com",
                PhoneNumber = "123456789",
                City = "Warsaw",
                Street = "Main Street",
                PostalCode = "00-001"
            }
        };
    
        // Act & Assert
        await Assert.ThrowsAsync<BadRequestException>(() =>
            service.CreateAdoptionAsync(dto, CancellationToken.None));
    }

    [Fact]
    public async Task DeleteAdoptionAsync_ShouldDeleteAdoption()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ShelterManagerContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
    
        var timeProvider = TimeProvider.System;
        await using var context = new ShelterManagerContext(options, timeProvider);

        var adoption = new Adoption
        {
            Id = Guid.NewGuid(),
            AnimalId = Guid.NewGuid(),
            Status = AdoptionStatus.Active,
            StartAdoptionProcess = timeProvider.GetUtcNow(),
            Person = new AdoptionPerson
            {
                Name = "Adopter",
                Surname = "Adopter",
                DocumentId = "AB123456",
                Pesel = "12345678901",
                Email = "adopter@adopter.com",
                PhoneNumber = "123456789",
                City = "Warsaw",
                Street = "Main Street",
                PostalCode = "00-001"
            }
        };
        context.Adoptions.Add(adoption);
        await context.SaveChangesAsync();
        
        var userContext = new Mock<IUserContextService>();
        var service = new AdoptionService(context, timeProvider, userContext.Object);
        
        // Act
        await service.DeleteAdoptionAsync(adoption.Id, CancellationToken.None);
        
        // Assert
        var result = await context.Adoptions.FirstOrDefaultAsync(x => x.Id == adoption.Id);
        Assert.Null(result);
    }
    
    [Fact]
    public async Task DeleteAdoptionAsync_ShouldThrowNotFound()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ShelterManagerContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var timeProvider = TimeProvider.System;
        await using var context = new ShelterManagerContext(options, timeProvider);
        var userContext = new Mock<IUserContextService>();
        var service = new AdoptionService(context, timeProvider, userContext.Object);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() =>
            service.DeleteAdoptionAsync(Guid.NewGuid(), CancellationToken.None));
    }
    
    [Fact]
    public async Task GetAdoptionAsync_ShouldReturnAdoptionDetails()
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

        var adoption = new Adoption
        {
            Id = Guid.NewGuid(),
            AnimalId = animal.Id,
            Status = AdoptionStatus.Active,
            StartAdoptionProcess = timeProvider.GetUtcNow(),
            Person = new AdoptionPerson
            {
                Name = "Old",
                Surname = "Adopter",
                DocumentId = "AB123456",
                Pesel = "12345678901",
                Email = "old@adopter.com",
                PhoneNumber = "123456789",
                City = "Warsaw",
                Street = "Main Street",
                PostalCode = "00-001"
            }
        };
        context.Adoptions.Add(adoption);
        
        await context.SaveChangesAsync();

        var userContext = new Mock<IUserContextService>();
        var service = new AdoptionService(context, timeProvider, userContext.Object);
        
        // Act
        var result = await service.GetAdoptionAsync(adoption.Id, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(adoption.Id, result.Id);
        Assert.Equal(animal.Name, result.AnimalName);
        Assert.Equal(adoption.Note, result.Note);
        Assert.Equal(adoption.Status, result.Status);
        Assert.NotNull(result.Person);
        Assert.Equal(adoption.Person.Name, result.Person.Name);
    }

    [Fact]
    public async Task GetAdoptionAsync_ShouldThrowNotFound()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ShelterManagerContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var timeProvider = TimeProvider.System;
        await using var context = new ShelterManagerContext(options, timeProvider);
        var userContext = new Mock<IUserContextService>();
        var service = new AdoptionService(context, timeProvider, userContext.Object);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() =>
            service.GetAdoptionAsync(Guid.NewGuid(), CancellationToken.None));
    }
    
    [Fact]
    public async Task ListAdoptionsAsync_ShouldReturnPaginatedAdoptions()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ShelterManagerContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var timeProvider = TimeProvider.System;
        await using var context = new ShelterManagerContext(options, timeProvider);

        for (var i = 1; i <= 15; i++)
        {
            var animal = new Animal
            {
                Id = Guid.NewGuid(),
                Name = $"Animal {i}",
                Age = 3,
                AdmissionDate = timeProvider.GetUtcNow(),
                Sex = Sex.Male,
                Status = AnimalStatus.InShelter,
                Description = "Friendly dog",
                BreedId = Guid.NewGuid()
            };
            context.Animals.Add(animal);

            var adoption = new Adoption
            {
                Id = Guid.NewGuid(),
                AnimalId = animal.Id,
                Status = AdoptionStatus.Active,
                StartAdoptionProcess = timeProvider.GetUtcNow(),
                Person = new AdoptionPerson
                {
                    Name = "Adopter",
                    Surname = "Adopter",
                    DocumentId = "AB123456",
                    Pesel = "12345678901",
                    Email = "adopter@adopter.com",
                    PhoneNumber = "123456789",
                    City = "Warsaw",
                    Street = "Main Street",
                    PostalCode = "00-001"
                }
            };
            context.Adoptions.Add(adoption);
        }
        await context.SaveChangesAsync();

        var userContext = new Mock<IUserContextService>();
        var service = new AdoptionService(context, timeProvider, userContext.Object);
        
        var filter = new AdoptionPageQueryFilter(null, null, null);

        // Act
        var result = await service.ListAdoptionsAsync(filter, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(15, result.TotalItemsCount);
        Assert.Equal(10, result.Items.Count);
        Assert.Equal(1, result.Page);
        Assert.Equal(10, result.PageSize);
        Assert.All(result.Items, dto => Assert.NotNull(dto.AnimalName));
    }

    [Fact]
    public async Task UpdateAdoptionAsync_ShouldUpdateAdoptionAndCreateEvent()
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
        
        var adoption = new Adoption
        {
            Id = Guid.NewGuid(),
            AnimalId = animal.Id,
            Status = AdoptionStatus.Active,
            StartAdoptionProcess = timeProvider.GetUtcNow(),
            Person = new AdoptionPerson
            {
                Name = "Adopter",
                Surname = "Adopter",
                DocumentId = "AB123456",
                Pesel = "12345678901",
                Email = "adopter@adopter.com",
                PhoneNumber = "123456789",
                City = "Warsaw",
                Street = "Main Street",
                PostalCode = "00-001"
            }
        };
        context.Adoptions.Add(adoption);
        await context.SaveChangesAsync();
        
        var userId = Guid.NewGuid();
        var userContext = new Mock<IUserContextService>();
        userContext.Setup(x => x.GetCurrentUserId()).Returns(userId);
        
        var service = new AdoptionService(context, timeProvider, userContext.Object);

        var dto = new UpdateAdoptionDto
        {
            Note = "Updated note",
            Status = AdoptionStatus.Approved,
            Event = new AdoptionEventDto
            {
                Title = "Adoption event",
                Location = "In shelter",
                PlannedAdoptionDate = timeProvider.GetUtcNow().AddDays(5),
                Description = "Adoption event description",
            }
        };
        
        // Act
        await service.UpdateAdoptionStatusAsync(adoption.Id, dto, CancellationToken.None);
        
        // Assert
        var updated = await context.Adoptions
            .Include(x => x.Animal)
            .FirstOrDefaultAsync(x => x.Id == adoption.Id);
        Assert.NotNull(updated);
        Assert.Equal(dto.Note, updated.Note);
        Assert.Equal(dto.Status, updated.Status);
        
        var adoptionEvent = await context.Events.Include(x => x.Animal).FirstOrDefaultAsync();
        Assert.NotNull(adoptionEvent);
        Assert.Equal(dto.Event.Title, adoptionEvent.Title);
        Assert.Equal(dto.Event.Location, adoptionEvent.Location);
        Assert.False(adoptionEvent.IsDone);
        Assert.Equal(adoptionEvent.UserId, userId);
    }
    
    [Fact]
    public async Task UpdateAdoptionAsync_ShouldUpdateAdoption()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ShelterManagerContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var timeProvider = TimeProvider.System;
        await using var context = new ShelterManagerContext(options, timeProvider);
        
        var adoption = new Adoption
        {
            Id = Guid.NewGuid(),
            AnimalId = Guid.NewGuid(),
            Status = AdoptionStatus.Active,
            StartAdoptionProcess = timeProvider.GetUtcNow(),
            Person = new AdoptionPerson
            {
                Name = "Adopter",
                Surname = "Adopter",
                DocumentId = "AB123456",
                Pesel = "12345678901",
                Email = "adopter@adopter.com",
                PhoneNumber = "123456789",
                City = "Warsaw",
                Street = "Main Street",
                PostalCode = "00-001"
            }
        };
        context.Adoptions.Add(adoption);
        await context.SaveChangesAsync();
        
        var userContext = new Mock<IUserContextService>();
        var service = new AdoptionService(context, timeProvider, userContext.Object);

        var dto = new UpdateAdoptionDto
        {
            Note = "Updated note",
            Status = AdoptionStatus.Rejected,
        };
        
        // Act
        await service.UpdateAdoptionStatusAsync(adoption.Id, dto, CancellationToken.None);
        
        // Assert
        var updated = await context.Adoptions
            .FirstOrDefaultAsync(x => x.Id == adoption.Id);
        
        Assert.NotNull(updated);
        Assert.Equal(dto.Note, updated.Note);
        Assert.Equal(dto.Status, updated.Status);
        
        var adoptionEvent = await context.Events.FirstOrDefaultAsync();
        Assert.Null(adoptionEvent);
    }
    
    [Fact]
    public async Task UpdateAdoptionAsync_ShouldThrowNotFound()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ShelterManagerContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var timeProvider = TimeProvider.System;
        await using var context = new ShelterManagerContext(options, timeProvider);
        
        var userContext = new Mock<IUserContextService>();
        var service = new AdoptionService(context, timeProvider, userContext.Object);

        var dto = new UpdateAdoptionDto
        {
            Note = "Updated note",
            Status = AdoptionStatus.Rejected,
        };
        
        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() =>
            service.UpdateAdoptionStatusAsync(Guid.NewGuid(), dto, CancellationToken.None));
    }
    
    [Fact]
    public async Task UpdateAdoptionAsync_ShouldThrowBadRequest()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ShelterManagerContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var timeProvider = TimeProvider.System;
        await using var context = new ShelterManagerContext(options, timeProvider);
        
        var adoption = new Adoption
        {
            Id = Guid.NewGuid(),
            AnimalId = Guid.NewGuid(),
            Status = AdoptionStatus.Approved,
            StartAdoptionProcess = timeProvider.GetUtcNow(),
            Person = new AdoptionPerson
            {
                Name = "Adopter",
                Surname = "Adopter",
                DocumentId = "AB123456",
                Pesel = "12345678901",
                Email = "adopter@adopter.com",
                PhoneNumber = "123456789",
                City = "Warsaw",
                Street = "Main Street",
                PostalCode = "00-001"
            }
        };
        context.Adoptions.Add(adoption);
        await context.SaveChangesAsync();
        
        var userContext = new Mock<IUserContextService>();
        var service = new AdoptionService(context, timeProvider, userContext.Object);

        var dto = new UpdateAdoptionDto
        {
            Note = "Updated note",
            Status = AdoptionStatus.Rejected,
        };
        
        // Act & Assert
        await Assert.ThrowsAsync<BadRequestException>(() =>
            service.UpdateAdoptionStatusAsync(adoption.Id, dto, CancellationToken.None));
    }
}
