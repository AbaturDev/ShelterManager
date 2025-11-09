using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using ShelterManager.Core.Exceptions;
using ShelterManager.Core.Services.Abstractions;
using ShelterManager.Database.Contexts;
using ShelterManager.Database.Entities;
using ShelterManager.Services.Dtos.Commons;
using ShelterManager.Services.Services;

namespace ShelterManager.Tests;

public class UserServiceTests
{
    private static UserManager<User> CreateUserManager(ShelterManagerContext context)
    {
        var userStore = new Mock<IUserStore<User>>();
        var userManager = new Mock<UserManager<User>>(
            userStore.Object,
            null!, null!, null!, null!, null!, null!, null!, null!);

        userManager.Setup(x => x.Users)
            .Returns(context.Users);

        return userManager.Object;
    }

    [Fact]
    public async Task ListUsersAsync_ShouldReturnPaginatedUsers()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ShelterManagerContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var timeProvider = TimeProvider.System;
        await using var context = new ShelterManagerContext(options, timeProvider);

        for (var i = 1; i <= 15; i++)
        {
            context.Users.Add(new User
            {
                Id = Guid.NewGuid(),
                UserName = $"user{i}@example.com",
                Email = $"user{i}@example.com",
                Name = $"First{i}",
                Surname = $"Last{i}"
            });
        }
        await context.SaveChangesAsync();

        var userManager = CreateUserManager(context);
        var userContext = new Mock<IUserContextService>();
        var logger = new Mock<ILogger<UserService>>();

        var userService = new UserService(userManager, userContext.Object, logger.Object);

        var pageQueryFilter = new PageQueryFilter();

        // Act
        var result = await userService.ListUsersAsync(pageQueryFilter, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(15, result.TotalItemsCount);
        Assert.Equal(10, result.Items.Count);
        Assert.Equal(pageQueryFilter.Page, result.Page);
        Assert.Equal(pageQueryFilter.PageSize, result.PageSize);
        Assert.All(result.Items, dto => Assert.NotNull(dto.Email));
    }

    [Fact]
    public async Task GetUserByIdAsync_ShouldReturnUser()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ShelterManagerContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var timeProvider = TimeProvider.System;
        await using var context = new ShelterManagerContext(options, timeProvider);

        var userId = Guid.NewGuid();
        var user = new User
        {
            Id = userId,
            UserName = "testuser@example.com",
            Email = "testuser@example.com",
            Name = "Test",
            Surname = "User"
        };
        context.Users.Add(user);
        await context.SaveChangesAsync();

        var userStore = new Mock<IUserStore<User>>();
        var userManager = new Mock<UserManager<User>>(
            userStore.Object,
            null!, null!, null!, null!, null!, null!, null!, null!);

        userManager.Setup(x => x.Users)
            .Returns(context.Users);

        userManager.Setup(x => x.GetRolesAsync(It.IsAny<User>()))
            .ReturnsAsync(new List<string> { "User" });

        var userContext = new Mock<IUserContextService>();
        var logger = new Mock<ILogger<UserService>>();

        var userService = new UserService(userManager.Object, userContext.Object, logger.Object);

        // Act
        var result = await userService.GetUserByIdAsync(userId, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(userId, result.Id);
        Assert.Equal(user.Email, result.Email);
        Assert.Equal(user.Name, result.Name);
        Assert.Equal(user.Surname, result.Surname);
        Assert.Equal("User", result.Role);
    }

    [Fact]
    public async Task GetUserByIdAsync_ShouldThrowNotFound()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ShelterManagerContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var timeProvider = TimeProvider.System;
        await using var context = new ShelterManagerContext(options, timeProvider);

        var userManager = CreateUserManager(context);
        var userContext = new Mock<IUserContextService>();
        var logger = new Mock<ILogger<UserService>>();

        var userService = new UserService(userManager, userContext.Object, logger.Object);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() =>
            userService.GetUserByIdAsync(Guid.NewGuid(), CancellationToken.None));
    }

    [Fact]
    public async Task GetCurrentUserAsync_ShouldReturnCurrentUser()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ShelterManagerContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var timeProvider = TimeProvider.System;
        await using var context = new ShelterManagerContext(options, timeProvider);

        var userId = Guid.NewGuid();
        var user = new User
        {
            Id = userId,
            UserName = "currentuser@example.com",
            Email = "currentuser@example.com",
            Name = "Current",
            Surname = "User"
        };
        context.Users.Add(user);
        await context.SaveChangesAsync();

        var userStore = new Mock<IUserStore<User>>();
        var userManager = new Mock<UserManager<User>>(
            userStore.Object,
            null!, null!, null!, null!, null!, null!, null!, null!);

        userManager.Setup(x => x.Users)
            .Returns(context.Users);

        userManager.Setup(x => x.GetRolesAsync(It.IsAny<User>()))
            .ReturnsAsync(new List<string> { "Admin" });

        var userContext = new Mock<IUserContextService>();
        userContext.Setup(x => x.GetCurrentUserId())
            .Returns(userId);

        var logger = new Mock<ILogger<UserService>>();

        var userService = new UserService(userManager.Object, userContext.Object, logger.Object);

        // Act
        var result = await userService.GetCurrentUserAsync(CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(userId, result.Id);
        Assert.Equal(user.Email, result.Email);
        Assert.Equal("Admin", result.Role);
    }

    [Fact]
    public async Task GetCurrentUserAsync_ShouldThrowNotFound()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ShelterManagerContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var timeProvider = TimeProvider.System;
        await using var context = new ShelterManagerContext(options, timeProvider);

        var userManager = CreateUserManager(context);

        var userContext = new Mock<IUserContextService>();
        userContext.Setup(x => x.GetCurrentUserId())
            .Returns(Guid.NewGuid());

        var logger = new Mock<ILogger<UserService>>();

        var userService = new UserService(userManager, userContext.Object, logger.Object);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() =>
            userService.GetCurrentUserAsync(CancellationToken.None));
    }

    [Fact]
    public async Task SoftDeleteUserAsync_ShouldMarkUserAsDeleted()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ShelterManagerContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var timeProvider = TimeProvider.System;
        await using var context = new ShelterManagerContext(options, timeProvider);

        var userId = Guid.NewGuid();
        var user = new User
        {
            Id = userId,
            UserName = "deleteuser@example.com",
            Email = "deleteuser@example.com",
            Name = "Delete",
            Surname = "User",
            IsDeleted = false
        };
        context.Users.Add(user);
        await context.SaveChangesAsync();

        var userStore = new Mock<IUserStore<User>>();
        var userManager = new Mock<UserManager<User>>(
            userStore.Object,
            null!, null!, null!, null!, null!, null!, null!, null!);

        userManager.Setup(x => x.Users)
            .Returns(context.Users);

        userManager.Setup(x => x.UpdateAsync(It.IsAny<User>()))
            .ReturnsAsync(IdentityResult.Success)
            .Callback<User>(u => u.IsDeleted = true);

        var userContext = new Mock<IUserContextService>();
        var logger = new Mock<ILogger<UserService>>();

        var userService = new UserService(userManager.Object, userContext.Object, logger.Object);

        // Act
        await userService.SoftDeleteUserAsync(userId, CancellationToken.None);

        // Assert
        var deletedUser = await context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        Assert.NotNull(deletedUser);
        Assert.True(deletedUser.IsDeleted);
    }

    [Fact]
    public async Task SoftDeleteUserAsync_ShouldThrowNotFound()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ShelterManagerContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var timeProvider = TimeProvider.System;
        await using var context = new ShelterManagerContext(options, timeProvider);

        var userManager = CreateUserManager(context);
        var userContext = new Mock<IUserContextService>();
        var logger = new Mock<ILogger<UserService>>();

        var userService = new UserService(userManager, userContext.Object, logger.Object);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() =>
            userService.SoftDeleteUserAsync(Guid.NewGuid(), CancellationToken.None));
    }
}