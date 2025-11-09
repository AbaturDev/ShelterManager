using Microsoft.EntityFrameworkCore;
using Moq;
using ShelterManager.Core.Exceptions;
using ShelterManager.Core.Services.Abstractions;
using ShelterManager.Database.Contexts;
using ShelterManager.Database.Entities;
using ShelterManager.Database.Entities.Owned;
using ShelterManager.Database.Enums;
using ShelterManager.Services.Dtos.Commons;
using ShelterManager.Services.Dtos.Events;
using ShelterManager.Services.Services;

namespace ShelterManager.Tests;

public class EventServiceTests
{
    [Fact]
    public async Task CreateEventAsync_ShouldCreateEvent()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ShelterManagerContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var timeProvider = TimeProvider.System;
        await using var context = new ShelterManagerContext(options, timeProvider);

        var userId = Guid.NewGuid();
        context.Users.Add(new User
        {
            Id = userId,
            UserName = "testuser@example.com",
            Email = "testuser@example.com",
            Name = "Test",
            Surname = "User"
        });
        await context.SaveChangesAsync();

        var userContext = new Mock<IUserContextService>();
        var service = new EventService(context, userContext.Object, timeProvider);

        var dto = new CreateEventDto
        {
            Title = "Test Event",
            Description = "Event Description",
            AnimalId = Guid.NewGuid(),
            UserId = userId,
            Date = timeProvider.GetUtcNow().AddDays(3),
            Location = "Shelter",
            Cost = new MoneyDto { Amount = 100, CurrencyCode = "PLN" }
        };

        // Act
        var eventId = await service.CreateEventAsync(dto, CancellationToken.None);

        // Assert
        Assert.NotEqual(Guid.Empty, eventId);
        
        var createdEvent = await context.Events.FirstOrDefaultAsync(x => x.Id == eventId);
        Assert.NotNull(createdEvent);
        Assert.Equal(dto.Title, createdEvent.Title);
        Assert.Equal(dto.AnimalId, createdEvent.AnimalId);
        Assert.Equal(userId, createdEvent.UserId);
        Assert.Equal(dto.Location, createdEvent.Location);
        Assert.False(createdEvent.IsDone);
        Assert.NotNull(createdEvent.Cost);
    }
    
    [Fact]
    public async Task CreateEventAsync_ShouldThrowBadRequest()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ShelterManagerContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var timeProvider = TimeProvider.System;
        await using var context = new ShelterManagerContext(options, timeProvider);

        var userContext = new Mock<IUserContextService>();
        var service = new EventService(context, userContext.Object, timeProvider);

        var dto = new CreateEventDto
        {
            Title = "Test Event",
            Description = "Event Description",
            AnimalId = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            Date = timeProvider.GetUtcNow().AddDays(3),
            Location = "Shelter",
            Cost = new MoneyDto { Amount = 100, CurrencyCode = "PLN" }
        };

        // Act & Assert
        await Assert.ThrowsAsync<BadRequestException>(() =>
            service.CreateEventAsync(dto, CancellationToken.None));
    }
    
    [Fact]
    public async Task DeleteEventAsync_ShouldDeleteEvent()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ShelterManagerContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var timeProvider = TimeProvider.System;
        await using var context = new ShelterManagerContext(options, timeProvider);

        var eventEntity = new Event
        {
            Id = Guid.NewGuid(),
            Title = "Test Event",
            Description = "Event Description",
            AnimalId = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            Date = timeProvider.GetUtcNow().AddDays(3),
            Location = "Shelter",
            Cost = new Money { Amount = 100, CurrencyCode = "PLN" },
            IsDone = false
        };
        context.Events.Add(eventEntity);
        await context.SaveChangesAsync();
        
        var userContext = new Mock<IUserContextService>();
        var service = new EventService(context, userContext.Object, timeProvider);

        // Act
        await service.DeleteEventAsync(eventEntity.Id, CancellationToken.None);
        
        // Assert
        var result = await context.Events.FirstOrDefaultAsync(x => x.Id == eventEntity.Id);
        Assert.Null(result);
    }
    
    [Fact]
    public async Task DeleteEventAsync_ShouldThrowNotFound()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ShelterManagerContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var timeProvider = TimeProvider.System;
        await using var context = new ShelterManagerContext(options, timeProvider);
        
        var userContext = new Mock<IUserContextService>();
        var service = new EventService(context, userContext.Object, timeProvider);
        
        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() =>
            service.DeleteEventAsync(Guid.NewGuid(), CancellationToken.None));
    }
    
    [Fact]
    public async Task GetEventAsync_ShouldReturnEvent()
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
        
        var eventEntity = new Event
        {
            Id = Guid.NewGuid(),
            Title = "Test Event",
            Description = "Event Description",
            AnimalId = animal.Id,
            UserId = Guid.NewGuid(),
            Date = timeProvider.GetUtcNow().AddDays(3),
            Location = "Shelter",
            Cost = new Money { Amount = 100, CurrencyCode = "PLN" },
            IsDone = false
        };
        context.Events.Add(eventEntity);
        await context.SaveChangesAsync();
        
        var userContext = new Mock<IUserContextService>();
        var service = new EventService(context, userContext.Object, timeProvider);

        // Act
        var result = await service.GetEventAsync(eventEntity.Id, CancellationToken.None);
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal(eventEntity.Id, result.Id);
        Assert.Equal(eventEntity.Title, result.Title);
        Assert.Equal(eventEntity.UserId, result.UserId);
        Assert.False(eventEntity.IsDone);
        Assert.Equal(animal.Id, result.AnimalId);
        if (eventEntity.Cost is not null)
        {
            Assert.NotNull(result.Cost);
        }
    }
    
    [Fact]
    public async Task GetEventAsync_ShouldThrowNotFound()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ShelterManagerContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var timeProvider = TimeProvider.System;
        await using var context = new ShelterManagerContext(options, timeProvider);
        
        var userContext = new Mock<IUserContextService>();
        var service = new EventService(context, userContext.Object, timeProvider);
        
        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() =>
            service.GetEventAsync(Guid.NewGuid(), CancellationToken.None));
    }
    
    [Fact]
    public async Task ListEventsAsync_ShouldReturnPaginatedEvents()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ShelterManagerContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var timeProvider = TimeProvider.System;
        await using var context = new ShelterManagerContext(options, timeProvider);

        var userId = Guid.NewGuid();
        context.Users.Add(new User
        {
            Id = userId,
            UserName = "testuser@example.com",
            Email = "testuser@example.com",
            Name = "Test",
            Surname = "User"
        });

        for (int i = 1; i <= 15; i++)
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

            var eventEntity = new Event
            {
                Id = Guid.NewGuid(),
                Title = $"Event {i}",
                Description = $"Event Description {i}",
                AnimalId = animal.Id,
                UserId = userId,
                Date = timeProvider.GetUtcNow().AddDays(i),
                Location = "Shelter",
                Cost = new Money { Amount = i * 10, CurrencyCode = "PLN" },
                IsDone = false
            };
            context.Events.Add(eventEntity);
        }

        await context.SaveChangesAsync();

        var userContext = new Mock<IUserContextService>();
        var service = new EventService(context, userContext.Object, timeProvider);

        var filter = new EventPageQueryFilter(null, null, null);
        
        // Act
        var result = await service.ListEventsAsync(filter, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(15, result.TotalItemsCount);
        Assert.Equal(10, result.Items.Count);
        Assert.Equal(1, result.Page);
        Assert.Equal(10, result.PageSize);
        Assert.All(result.Items, dto => Assert.NotNull(dto.Title));
        Assert.All(result.Items, dto => Assert.NotNull(dto.AnimalName));
    }

    [Fact]
    public async Task UpdateEventAsync_ShouldUpdateEvent()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ShelterManagerContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var timeProvider = TimeProvider.System;
        await using var context = new ShelterManagerContext(options, timeProvider);

        var userId = Guid.NewGuid();
        context.Users.Add(new User
        {
            Id = userId,
            UserName = "testuser@example.com",
            Email = "testuser@example.com",
            Name = "Test",
            Surname = "User"
        });

        var eventEntity = new Event
        {
            Id = Guid.NewGuid(),
            Title = "Old Event",
            Description = "Old Description",
            AnimalId = Guid.NewGuid(),
            UserId = userId,
            Date = timeProvider.GetUtcNow().AddDays(3),
            Location = "Old Location",
            Cost = new Money { Amount = 50, CurrencyCode = "PLN" },
            IsDone = false
        };
        context.Events.Add(eventEntity);
        await context.SaveChangesAsync();

        var userContext = new Mock<IUserContextService>();
        var service = new EventService(context, userContext.Object, timeProvider);

        var dto = new UpdateEventDto
        {
            Title = "Updated Event",
            Description = "Updated Description",
            UserId = userId,
            Date = timeProvider.GetUtcNow().AddDays(5),
            Location = "Updated Location",
            Cost = new MoneyDto { Amount = 100, CurrencyCode = "PLN" }
        };

        // Act
        await service.UpdateEventAsync(eventEntity.Id, dto, CancellationToken.None);

        // Assert
        var updatedEvent = await context.Events.FirstOrDefaultAsync(x => x.Id == eventEntity.Id);
        Assert.NotNull(updatedEvent);
        Assert.Equal(dto.Title, updatedEvent.Title);
        Assert.Equal(dto.Description, updatedEvent.Description);
        Assert.Equal(dto.Location, updatedEvent.Location);
        Assert.Equal(dto.UserId, updatedEvent.UserId);
        Assert.Equal(dto.Cost.Amount, updatedEvent.Cost?.Amount);
        Assert.Equal(dto.Cost.CurrencyCode, updatedEvent.Cost?.CurrencyCode);
    }

    [Fact]
    public async Task UpdateEventAsync_ShouldThrowNotFound()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ShelterManagerContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var timeProvider = TimeProvider.System;
        await using var context = new ShelterManagerContext(options, timeProvider);

        var userContext = new Mock<IUserContextService>();
        var service = new EventService(context, userContext.Object, timeProvider);

        var dto = new UpdateEventDto
        {
            Title = "Updated Event",
            Description = "Updated Description",
            UserId = Guid.NewGuid(),
            Date = timeProvider.GetUtcNow().AddDays(5),
            Location = "Updated Location",
            Cost = new MoneyDto { Amount = 100, CurrencyCode = "PLN" }
        };

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() =>
            service.UpdateEventAsync(Guid.NewGuid(), dto, CancellationToken.None));
    }

    [Fact]
    public async Task UpdateEventAsync_ShouldThrowBadRequest_WhenUserDoesNotExist()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ShelterManagerContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var timeProvider = TimeProvider.System;
        await using var context = new ShelterManagerContext(options, timeProvider);

        var eventEntity = new Event
        {
            Id = Guid.NewGuid(),
            Title = "Old Event",
            Description = "Old Description",
            AnimalId = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            Date = timeProvider.GetUtcNow().AddDays(3),
            Location = "Old Location",
            Cost = new Money { Amount = 50, CurrencyCode = "PLN" },
            IsDone = false
        };
        context.Events.Add(eventEntity);
        await context.SaveChangesAsync();

        var userContext = new Mock<IUserContextService>();
        var service = new EventService(context, userContext.Object, timeProvider);

        var dto = new UpdateEventDto
        {
            Title = "Updated Event",
            Description = "Updated Description",
            UserId = Guid.NewGuid(),
            Date = timeProvider.GetUtcNow().AddDays(5),
            Location = "Updated Location",
            Cost = new MoneyDto { Amount = 100, CurrencyCode = "PLN" }
        };

        // Act & Assert
        await Assert.ThrowsAsync<BadRequestException>(() =>
            service.UpdateEventAsync(eventEntity.Id, dto, CancellationToken.None));
    }

    [Fact]
    public async Task EndEventAsync_ShouldEndEvent()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ShelterManagerContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var timeProvider = TimeProvider.System;
        await using var context = new ShelterManagerContext(options, timeProvider);

        var eventEntity = new Event
        {
            Id = Guid.NewGuid(),
            Title = "Test Event",
            Description = "Event Description",
            AnimalId = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            Date = timeProvider.GetUtcNow().AddDays(3),
            Location = "Shelter",
            Cost = new Money { Amount = 100, CurrencyCode = "PLN" },
            IsDone = false
        };
        context.Events.Add(eventEntity);
        await context.SaveChangesAsync();
        
        var userId = Guid.NewGuid();
        var userContext = new Mock<IUserContextService>();
        userContext.Setup(x => x.GetCurrentUserId()).Returns(userId);
        
        var service = new EventService(context, userContext.Object, timeProvider);

        // Act
        await service.EndEventAsync(eventEntity.Id, CancellationToken.None);
        
        // Assert
        var endedEvent = await context.Events.FirstOrDefaultAsync(x => x.Id == eventEntity.Id);
        Assert.NotNull(endedEvent);
        Assert.True(eventEntity.IsDone);
        Assert.NotNull(eventEntity.CompletedAt);
        Assert.Equal(eventEntity.CompletedByUserId, userId);
    }
    
    [Fact]
    public async Task EndEventAsync_ShouldThrowBadRequest()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ShelterManagerContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var timeProvider = TimeProvider.System;
        await using var context = new ShelterManagerContext(options, timeProvider);

        var eventEntity = new Event
        {
            Id = Guid.NewGuid(),
            Title = "Test Event",
            Description = "Event Description",
            AnimalId = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            CompletedByUserId = Guid.NewGuid(),
            CompletedAt = timeProvider.GetUtcNow().AddDays(3),
            Date = timeProvider.GetUtcNow().AddDays(3),
            Location = "Shelter",
            Cost = new Money { Amount = 100, CurrencyCode = "PLN" },
            IsDone = true
        };
        context.Events.Add(eventEntity);
        await context.SaveChangesAsync();
        
        var userContext = new Mock<IUserContextService>();
        var service = new EventService(context, userContext.Object, timeProvider);

        // Act & Assert
        await Assert.ThrowsAsync<BadRequestException>(() =>
            service.EndEventAsync(eventEntity.Id, CancellationToken.None));
    }
    
    [Fact]
    public async Task EndEventAsync_ShouldThrowNotFound()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ShelterManagerContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var timeProvider = TimeProvider.System;
        await using var context = new ShelterManagerContext(options, timeProvider);
        
        var userContext = new Mock<IUserContextService>();
        var service = new EventService(context, userContext.Object, timeProvider);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() =>
            service.EndEventAsync(Guid.NewGuid(), CancellationToken.None));
    }
}