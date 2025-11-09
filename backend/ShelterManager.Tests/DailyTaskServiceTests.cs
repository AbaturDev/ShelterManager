using Microsoft.EntityFrameworkCore;
using Moq;
using ShelterManager.Core.Exceptions;
using ShelterManager.Core.Services.Abstractions;
using ShelterManager.Database.Contexts;
using ShelterManager.Database.Entities;
using ShelterManager.Services.Dtos.DailyTask;
using ShelterManager.Services.Services;

namespace ShelterManager.Tests;

public class DailyTaskServiceTests
{
    [Fact]
    public async Task GetDailyTaskAsync_ShouldReturnDailyTask()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ShelterManagerContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var timeProvider = TimeProvider.System;
        await using var context = new ShelterManagerContext(options, timeProvider);

        var animalId = Guid.NewGuid();
        var date = DateOnly.FromDateTime(timeProvider.GetUtcNow().UtcDateTime);
        
        var user = new User
        {
            Id = Guid.NewGuid(),
            UserName = "test@example.com",
            Email = "test@example.com",
            Name = "Test",
            Surname = "User"
        };
        context.Users.Add(user);

        var dailyTask = new DailyTask
        {
            Id = Guid.NewGuid(),
            AnimalId = animalId,
            Date = date
        };
        context.DailyTasks.Add(dailyTask);

        var entry = new DailyTaskEntry
        {
            Id = Guid.NewGuid(),
            DailyTaskId = dailyTask.Id,
            Title = "Feed the animal",
            Description = "Give food and water",
            IsCompleted = false,
            UserId = user.Id
        };
        context.DailyTaskEntries.Add(entry);
        await context.SaveChangesAsync();

        var userContext = new Mock<IUserContextService>();
        var service = new DailyTaskService(context, timeProvider, userContext.Object);

        // Act
        var result = await service.GetDailyTaskAsync(animalId, date, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(dailyTask.Id, result.Id);
        Assert.Equal(animalId, result.AnimalId);
        Assert.Equal(date, result.Date);
        Assert.Single(result.Entries);
        Assert.Equal(entry.Title, result.Entries.First().Title);
        Assert.Equal("Test User", result.Entries.First().UserDisplayName);
    }

    [Fact]
    public async Task GetDailyTaskAsync_ShouldThrowNotFound()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ShelterManagerContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var timeProvider = TimeProvider.System;
        await using var context = new ShelterManagerContext(options, timeProvider);

        var userContext = new Mock<IUserContextService>();
        var service = new DailyTaskService(context, timeProvider, userContext.Object);

        var animalId = Guid.NewGuid();
        var date = DateOnly.FromDateTime(timeProvider.GetUtcNow().UtcDateTime);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() =>
            service.GetDailyTaskAsync(animalId, date, CancellationToken.None));
    }

    [Fact]
    public async Task AddDailyTaskEntryAsync_ShouldCreateNewDailyTaskAndEntry()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ShelterManagerContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var timeProvider = TimeProvider.System;
        await using var context = new ShelterManagerContext(options, timeProvider);

        var userContext = new Mock<IUserContextService>();
        var service = new DailyTaskService(context, timeProvider, userContext.Object);

        var animalId = Guid.NewGuid();
        var dto = new AddDailyTaskEntryDto
        {
            Title = "Walk the dog",
            Description = "30 minutes walk"
        };

        // Act
        await service.AddDailyTaskEntryAsync(animalId, dto, CancellationToken.None);

        // Assert
        var date = DateOnly.FromDateTime(timeProvider.GetUtcNow().UtcDateTime);
        var dailyTask = await context.DailyTasks
            .Include(x => x.Entries)
            .FirstOrDefaultAsync(x => x.AnimalId == animalId && x.Date == date);

        Assert.NotNull(dailyTask);
        Assert.Equal(animalId, dailyTask.AnimalId);
        Assert.Equal(date, dailyTask.Date);
        Assert.Single(dailyTask.Entries);
        Assert.Equal(dto.Title, dailyTask.Entries.First().Title);
        Assert.Equal(dto.Description, dailyTask.Entries.First().Description);
        Assert.False(dailyTask.Entries.First().IsCompleted);
    }

    [Fact]
    public async Task AddDailyTaskEntryAsync_ShouldAddEntryToExistingDailyTask()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ShelterManagerContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var timeProvider = TimeProvider.System;
        await using var context = new ShelterManagerContext(options, timeProvider);

        var animalId = Guid.NewGuid();
        var date = DateOnly.FromDateTime(timeProvider.GetUtcNow().UtcDateTime);

        var dailyTask = new DailyTask
        {
            Id = Guid.NewGuid(),
            AnimalId = animalId,
            Date = date
        };
        context.DailyTasks.Add(dailyTask);

        var existingEntry = new DailyTaskEntry
        {
            Id = Guid.NewGuid(),
            DailyTaskId = dailyTask.Id,
            Title = "Existing task",
            Description = "Description",
            IsCompleted = false
        };
        context.DailyTaskEntries.Add(existingEntry);
        await context.SaveChangesAsync();

        var userContext = new Mock<IUserContextService>();
        var service = new DailyTaskService(context, timeProvider, userContext.Object);

        var dto = new AddDailyTaskEntryDto
        {
            Title = "New task",
            Description = "New description"
        };

        // Act
        await service.AddDailyTaskEntryAsync(animalId, dto, CancellationToken.None);

        // Assert
        var updatedTask = await context.DailyTasks
            .Include(x => x.Entries)
            .FirstOrDefaultAsync(x => x.Id == dailyTask.Id);

        Assert.NotNull(updatedTask);
        Assert.Equal(2, updatedTask.Entries.Count);
        Assert.Contains(updatedTask.Entries, e => e.Title == "New task");
    }

    [Fact]
    public async Task RemoveDailyTaskEntryAsync_ShouldRemoveEntry()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ShelterManagerContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var timeProvider = TimeProvider.System;
        await using var context = new ShelterManagerContext(options, timeProvider);

        var animalId = Guid.NewGuid();
        var date = DateOnly.FromDateTime(timeProvider.GetUtcNow().UtcDateTime);

        var dailyTask = new DailyTask
        {
            Id = Guid.NewGuid(),
            AnimalId = animalId,
            Date = date
        };
        context.DailyTasks.Add(dailyTask);

        var entry = new DailyTaskEntry
        {
            Id = Guid.NewGuid(),
            DailyTaskId = dailyTask.Id,
            Title = "Task to remove",
            Description = "Description",
            IsCompleted = false
        };
        context.DailyTaskEntries.Add(entry);
        await context.SaveChangesAsync();

        var userContext = new Mock<IUserContextService>();
        var service = new DailyTaskService(context, timeProvider, userContext.Object);

        // Act
        await service.RemoveDailyTaskEntryAsync(animalId, entry.Id, CancellationToken.None);

        // Assert
        var removedEntry = await context.DailyTaskEntries.FirstOrDefaultAsync(x => x.Id == entry.Id);
        Assert.Null(removedEntry);
    }

    [Fact]
    public async Task RemoveDailyTaskEntryAsync_ShouldThrowNotFound()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ShelterManagerContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var timeProvider = TimeProvider.System;
        await using var context = new ShelterManagerContext(options, timeProvider);

        var userContext = new Mock<IUserContextService>();
        var service = new DailyTaskService(context, timeProvider, userContext.Object);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() =>
            service.RemoveDailyTaskEntryAsync(Guid.NewGuid(), Guid.NewGuid(), CancellationToken.None));
    }

    [Fact]
    public async Task RemoveDailyTaskEntryAsync_ShouldThrowBadRequest_WhenEntryIsFromDifferentDay()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ShelterManagerContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var timeProvider = TimeProvider.System;
        await using var context = new ShelterManagerContext(options, timeProvider);

        var animalId = Guid.NewGuid();
        var pastDate = DateOnly.FromDateTime(timeProvider.GetUtcNow().AddDays(-1).UtcDateTime);

        var dailyTask = new DailyTask
        {
            Id = Guid.NewGuid(),
            AnimalId = animalId,
            Date = pastDate
        };
        context.DailyTasks.Add(dailyTask);

        var entry = new DailyTaskEntry
        {
            Id = Guid.NewGuid(),
            DailyTaskId = dailyTask.Id,
            Title = "Old task",
            Description = "Description",
            IsCompleted = false
        };
        context.DailyTaskEntries.Add(entry);
        await context.SaveChangesAsync();

        var userContext = new Mock<IUserContextService>();
        var service = new DailyTaskService(context, timeProvider, userContext.Object);

        // Act & Assert
        await Assert.ThrowsAsync<BadRequestException>(() =>
            service.RemoveDailyTaskEntryAsync(animalId, entry.Id, CancellationToken.None));
    }

    [Fact]
    public async Task EndDailyTaskEntryAsync_ShouldMarkEntryAsCompleted()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ShelterManagerContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var timeProvider = TimeProvider.System;
        await using var context = new ShelterManagerContext(options, timeProvider);

        var animalId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var date = DateOnly.FromDateTime(timeProvider.GetUtcNow().UtcDateTime);

        var dailyTask = new DailyTask
        {
            Id = Guid.NewGuid(),
            AnimalId = animalId,
            Date = date
        };
        context.DailyTasks.Add(dailyTask);

        var entry = new DailyTaskEntry
        {
            Id = Guid.NewGuid(),
            DailyTaskId = dailyTask.Id,
            Title = "Task to complete",
            Description = "Description",
            IsCompleted = false
        };
        context.DailyTaskEntries.Add(entry);
        await context.SaveChangesAsync();

        var userContext = new Mock<IUserContextService>();
        userContext.Setup(x => x.GetCurrentUserId()).Returns(userId);

        var service = new DailyTaskService(context, timeProvider, userContext.Object);

        // Act
        await service.EndDailyTaskEntryAsync(animalId, entry.Id, CancellationToken.None);

        // Assert
        var completedEntry = await context.DailyTaskEntries.FirstOrDefaultAsync(x => x.Id == entry.Id);
        Assert.NotNull(completedEntry);
        Assert.True(completedEntry.IsCompleted);
        Assert.NotNull(completedEntry.CompletedAt);
        Assert.Equal(userId, completedEntry.UserId);
    }

    [Fact]
    public async Task EndDailyTaskEntryAsync_ShouldThrowNotFound()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ShelterManagerContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var timeProvider = TimeProvider.System;
        await using var context = new ShelterManagerContext(options, timeProvider);

        var userContext = new Mock<IUserContextService>();
        var service = new DailyTaskService(context, timeProvider, userContext.Object);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() =>
            service.EndDailyTaskEntryAsync(Guid.NewGuid(), Guid.NewGuid(), CancellationToken.None));
    }

    [Fact]
    public async Task EndDailyTaskEntryAsync_ShouldThrowBadRequest_WhenEntryIsFromDifferentDay()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ShelterManagerContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var timeProvider = TimeProvider.System;
        await using var context = new ShelterManagerContext(options, timeProvider);

        var animalId = Guid.NewGuid();
        var pastDate = DateOnly.FromDateTime(timeProvider.GetUtcNow().AddDays(-1).UtcDateTime);

        var dailyTask = new DailyTask
        {
            Id = Guid.NewGuid(),
            AnimalId = animalId,
            Date = pastDate
        };
        context.DailyTasks.Add(dailyTask);

        var entry = new DailyTaskEntry
        {
            Id = Guid.NewGuid(),
            DailyTaskId = dailyTask.Id,
            Title = "Old task",
            Description = "Description",
            IsCompleted = false
        };
        context.DailyTaskEntries.Add(entry);
        await context.SaveChangesAsync();

        var userContext = new Mock<IUserContextService>();
        var service = new DailyTaskService(context, timeProvider, userContext.Object);

        // Act & Assert
        await Assert.ThrowsAsync<BadRequestException>(() =>
            service.EndDailyTaskEntryAsync(animalId, entry.Id, CancellationToken.None));
    }

    [Fact]
    public async Task EndDailyTaskEntryAsync_ShouldThrowBadRequest_WhenEntryIsAlreadyCompleted()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ShelterManagerContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var timeProvider = TimeProvider.System;
        await using var context = new ShelterManagerContext(options, timeProvider);

        var animalId = Guid.NewGuid();
        var date = DateOnly.FromDateTime(timeProvider.GetUtcNow().UtcDateTime);

        var dailyTask = new DailyTask
        {
            Id = Guid.NewGuid(),
            AnimalId = animalId,
            Date = date
        };
        context.DailyTasks.Add(dailyTask);

        var entry = new DailyTaskEntry
        {
            Id = Guid.NewGuid(),
            DailyTaskId = dailyTask.Id,
            Title = "Completed task",
            Description = "Description",
            IsCompleted = true,
            CompletedAt = timeProvider.GetUtcNow(),
            UserId = Guid.NewGuid()
        };
        context.DailyTaskEntries.Add(entry);
        await context.SaveChangesAsync();

        var userContext = new Mock<IUserContextService>();
        var service = new DailyTaskService(context, timeProvider, userContext.Object);

        // Act & Assert
        await Assert.ThrowsAsync<BadRequestException>(() =>
            service.EndDailyTaskEntryAsync(animalId, entry.Id, CancellationToken.None));
    }

    [Fact]
    public async Task AddDefaultDailyTaskEntryAsync_ShouldCreateDefaultEntry()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ShelterManagerContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var timeProvider = TimeProvider.System;
        await using var context = new ShelterManagerContext(options, timeProvider);

        var animalId = Guid.NewGuid();
        context.Animals.Add(new Animal
        {
            Id = animalId,
            Name = "Test Animal",
            Age = 3,
            AdmissionDate = timeProvider.GetUtcNow(),
            Sex = Database.Enums.Sex.Male,
            Status = Database.Enums.AnimalStatus.InShelter,
            BreedId = Guid.NewGuid()
        });
        await context.SaveChangesAsync();

        var userContext = new Mock<IUserContextService>();
        var service = new DailyTaskService(context, timeProvider, userContext.Object);

        var dto = new AddDefaultDailyTaskEntryDto
        {
            Title = "Default feeding",
            Description = "Feed twice a day"
        };

        // Act
        await service.AddDefaultDailyTaskEntryAsync(animalId, dto, CancellationToken.None);

        // Assert
        var defaultEntry = await context.DailyTaskDefaultEntries
            .FirstOrDefaultAsync(x => x.AnimalId == animalId);

        Assert.NotNull(defaultEntry);
        Assert.Equal(dto.Title, defaultEntry.Title);
        Assert.Equal(dto.Description, defaultEntry.Description);
        Assert.Equal(animalId, defaultEntry.AnimalId);
    }

    [Fact]
    public async Task AddDefaultDailyTaskEntryAsync_ShouldThrowBadRequest_WhenAnimalDoesNotExist()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ShelterManagerContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var timeProvider = TimeProvider.System;
        await using var context = new ShelterManagerContext(options, timeProvider);

        var userContext = new Mock<IUserContextService>();
        var service = new DailyTaskService(context, timeProvider, userContext.Object);

        var dto = new AddDefaultDailyTaskEntryDto
        {
            Title = "Default task",
            Description = "Description"
        };

        // Act & Assert
        await Assert.ThrowsAsync<BadRequestException>(() =>
            service.AddDefaultDailyTaskEntryAsync(Guid.NewGuid(), dto, CancellationToken.None));
    }

    [Fact]
    public async Task RemoveDefaultDailyTaskEntryAsync_ShouldRemoveDefaultEntry()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ShelterManagerContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var timeProvider = TimeProvider.System;
        await using var context = new ShelterManagerContext(options, timeProvider);

        var animalId = Guid.NewGuid();
        var defaultEntry = new DailyTaskDefaultEntry
        {
            Id = Guid.NewGuid(),
            AnimalId = animalId,
            Title = "Default task",
            Description = "Description"
        };
        context.DailyTaskDefaultEntries.Add(defaultEntry);
        await context.SaveChangesAsync();

        var userContext = new Mock<IUserContextService>();
        var service = new DailyTaskService(context, timeProvider, userContext.Object);

        // Act
        await service.RemoveDefaultDailyTaskEntryAsync(animalId, defaultEntry.Id, CancellationToken.None);

        // Assert
        var removedEntry = await context.DailyTaskDefaultEntries
            .FirstOrDefaultAsync(x => x.Id == defaultEntry.Id);
        Assert.Null(removedEntry);
    }

    [Fact]
    public async Task RemoveDefaultDailyTaskEntryAsync_ShouldThrowNotFound()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ShelterManagerContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var timeProvider = TimeProvider.System;
        await using var context = new ShelterManagerContext(options, timeProvider);

        var userContext = new Mock<IUserContextService>();
        var service = new DailyTaskService(context, timeProvider, userContext.Object);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() =>
            service.RemoveDefaultDailyTaskEntryAsync(Guid.NewGuid(), Guid.NewGuid(), CancellationToken.None));
    }

    [Fact]
    public async Task GetDefaultDailyTaskEntriesAsync_ShouldReturnAllDefaultEntries()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ShelterManagerContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var timeProvider = TimeProvider.System;
        await using var context = new ShelterManagerContext(options, timeProvider);

        var animalId = Guid.NewGuid();

        for (int i = 1; i <= 5; i++)
        {
            context.DailyTaskDefaultEntries.Add(new DailyTaskDefaultEntry
            {
                Id = Guid.NewGuid(),
                AnimalId = animalId,
                Title = $"Default task {i}",
                Description = $"Description {i}"
            });
        }
        await context.SaveChangesAsync();

        var userContext = new Mock<IUserContextService>();
        var service = new DailyTaskService(context, timeProvider, userContext.Object);

        // Act
        var result = await service.GetDefaultDailyTaskEntriesAsync(animalId, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(5, result.Count);
        Assert.All(result, dto => Assert.Equal(animalId, dto.AnimalId));
        Assert.All(result, dto => Assert.NotNull(dto.Title));
    }

    [Fact]
    public async Task UpdateDefaultDailyTaskEntryAsync_ShouldUpdateDefaultEntry()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ShelterManagerContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var timeProvider = TimeProvider.System;
        await using var context = new ShelterManagerContext(options, timeProvider);

        var animalId = Guid.NewGuid();
        var defaultEntry = new DailyTaskDefaultEntry
        {
            Id = Guid.NewGuid(),
            AnimalId = animalId,
            Title = "Old title",
            Description = "Old description"
        };
        context.DailyTaskDefaultEntries.Add(defaultEntry);
        await context.SaveChangesAsync();

        var userContext = new Mock<IUserContextService>();
        var service = new DailyTaskService(context, timeProvider, userContext.Object);

        var dto = new UpdateDefaultDailyTaskEntryDto
        {
            Title = "Updated title",
            Description = "Updated description"
        };

        // Act
        await service.UpdateDefaultDailyTaskEntryAsync(animalId, defaultEntry.Id, dto, CancellationToken.None);

        // Assert
        var updatedEntry = await context.DailyTaskDefaultEntries
            .FirstOrDefaultAsync(x => x.Id == defaultEntry.Id);

        Assert.NotNull(updatedEntry);
        Assert.Equal(dto.Title, updatedEntry.Title);
        Assert.Equal(dto.Description, updatedEntry.Description);
    }

    [Fact]
    public async Task UpdateDefaultDailyTaskEntryAsync_ShouldThrowNotFound()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ShelterManagerContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var timeProvider = TimeProvider.System;
        await using var context = new ShelterManagerContext(options, timeProvider);

        var userContext = new Mock<IUserContextService>();
        var service = new DailyTaskService(context, timeProvider, userContext.Object);

        var dto = new UpdateDefaultDailyTaskEntryDto
        {
            Title = "Updated title",
            Description = "Updated description"
        };

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() =>
            service.UpdateDefaultDailyTaskEntryAsync(Guid.NewGuid(), Guid.NewGuid(), dto, CancellationToken.None));
    }
}