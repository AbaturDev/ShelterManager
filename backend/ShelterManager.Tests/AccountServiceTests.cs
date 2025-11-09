using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Moq;
using ShelterManager.Core.Exceptions;
using ShelterManager.Core.Options;
using ShelterManager.Core.Services.Abstractions;
using ShelterManager.Database.Contexts;
using ShelterManager.Database.Entities;
using ShelterManager.Services.Dtos.Accounts;
using ShelterManager.Services.Services;

namespace ShelterManager.Tests;

public class AccountServiceTests
{
    private static Mock<UserManager<User>> CreateMockUserManager()
    {
        var userStore = new Mock<IUserStore<User>>();
        return new Mock<UserManager<User>>(
            userStore.Object,
            null!, null!, null!, null!, null!, null!, null!, null!);
    }

    private static IOptions<JwtOptions> CreateJwtOptions()
    {
        return Options.Create(new JwtOptions
        {
            Secret = "ThisIsAVeryLongSecretKeyForTestingPurposesOnly123456789",
            Issuer = "TestIssuer",
            Audience = "TestAudience"
        });
    }

    private static IOptions<FrontendOptions> CreateFrontendOptions()
    {
        return Options.Create(new FrontendOptions
        {
            Url = "https://test-frontend.com"
        });
    }

    [Fact]
    public async Task LoginAsync_ShouldReturnTokens_WhenCredentialsAreValid()
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
            Email = "test@example.com",
            UserName = "test@example.com",
            Name = "Test",
            Surname = "User"
        };

        var userManager = CreateMockUserManager();
        userManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync(user);
        userManager.Setup(x => x.CheckPasswordAsync(It.IsAny<User>(), It.IsAny<string>()))
            .ReturnsAsync(true);
        userManager.Setup(x => x.GetRolesAsync(It.IsAny<User>()))
            .ReturnsAsync(new List<string> { "User" });

        var emailService = new Mock<IEmailService>();
        var templateService = new Mock<ITemplateService>();
        var jwtOptions = CreateJwtOptions();
        var frontendOptions = CreateFrontendOptions();

        var accountService = new AccountService(
            userManager.Object,
            jwtOptions,
            timeProvider,
            emailService.Object,
            templateService.Object,
            context,
            frontendOptions);

        var request = new LoginRequest
        {
            Email = "test@example.com",
            Password = "Password123!"
        };

        // Act
        var result = await accountService.LoginAsync(request);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result.JwtToken);
        Assert.NotEmpty(result.RefreshToken);
    }

    [Fact]
    public async Task LoginAsync_ShouldThrowBadRequest_WhenUserNotFound()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ShelterManagerContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var timeProvider = TimeProvider.System;
        await using var context = new ShelterManagerContext(options, timeProvider);

        var userManager = CreateMockUserManager();
        userManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync((User?)null);

        var emailService = new Mock<IEmailService>();
        var templateService = new Mock<ITemplateService>();
        var jwtOptions = CreateJwtOptions();
        var frontendOptions = CreateFrontendOptions();

        var accountService = new AccountService(
            userManager.Object,
            jwtOptions,
            timeProvider,
            emailService.Object,
            templateService.Object,
            context,
            frontendOptions);

        var request = new LoginRequest
        {
            Email = "nonexistent@example.com",
            Password = "Password123!"
        };

        // Act & Assert
        await Assert.ThrowsAsync<BadRequestException>(() =>
            accountService.LoginAsync(request));
    }

    [Fact]
    public async Task LoginAsync_ShouldThrowBadRequest_WhenPasswordIsInvalid()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ShelterManagerContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var timeProvider = TimeProvider.System;
        await using var context = new ShelterManagerContext(options, timeProvider);

        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = "test@example.com",
            UserName = "test@example.com",
            Name = "Test",
            Surname = "Test"
        };

        var userManager = CreateMockUserManager();
        userManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync(user);
        userManager.Setup(x => x.CheckPasswordAsync(It.IsAny<User>(), It.IsAny<string>()))
            .ReturnsAsync(false);

        var emailService = new Mock<IEmailService>();
        var templateService = new Mock<ITemplateService>();
        var jwtOptions = CreateJwtOptions();
        var frontendOptions = CreateFrontendOptions();

        var accountService = new AccountService(
            userManager.Object,
            jwtOptions,
            timeProvider,
            emailService.Object,
            templateService.Object,
            context,
            frontendOptions);

        var request = new LoginRequest
        {
            Email = "test@example.com",
            Password = "WrongPassword!"
        };

        // Act & Assert
        await Assert.ThrowsAsync<BadRequestException>(() =>
            accountService.LoginAsync(request));
    }

    [Fact]
    public async Task RegisterAsync_ShouldCreateUser_WhenDataIsValid()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ShelterManagerContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var timeProvider = TimeProvider.System;
        await using var context = new ShelterManagerContext(options, timeProvider);

        var userManager = CreateMockUserManager();
        userManager.Setup(x => x.Users)
            .Returns(context.Users);
        userManager.Setup(x => x.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Success);
        userManager.Setup(x => x.AddToRoleAsync(It.IsAny<User>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Success);

        var emailService = new Mock<IEmailService>();
        var templateService = new Mock<ITemplateService>();
        templateService.Setup(x => x.LoadTemplate(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()))
            .Returns("<html>Test email</html>");

        var jwtOptions = CreateJwtOptions();
        var frontendOptions = CreateFrontendOptions();

        var accountService = new AccountService(
            userManager.Object,
            jwtOptions,
            timeProvider,
            emailService.Object,
            templateService.Object,
            context,
            frontendOptions);

        var request = new RegisterRequest
        {
            Email = "newuser@example.com",
            Name = "New",
            Surname = "User",
            Role = "User"
        };

        // Act
        await accountService.RegisterAsync(request, "en");

        // Assert
        userManager.Verify(x => x.CreateAsync(It.IsAny<User>(), It.IsAny<string>()), Times.Once);
        userManager.Verify(x => x.AddToRoleAsync(It.IsAny<User>(), "User"), Times.Once);
        emailService.Verify(x => x.SendEmailAsync(
            "newuser@example.com",
            "New User",
            It.IsAny<string>(),
            It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task RegisterAsync_ShouldThrowBadRequest_WhenEmailExists()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ShelterManagerContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var timeProvider = TimeProvider.System;
        await using var context = new ShelterManagerContext(options, timeProvider);

        context.Users.Add(new User
        {
            Id = Guid.NewGuid(),
            Email = "existing@example.com",
            UserName = "existing@example.com",
            Name = "Test",
            Surname = "Test"
        });
        await context.SaveChangesAsync();

        var userManager = CreateMockUserManager();
        userManager.Setup(x => x.Users)
            .Returns(context.Users);

        var emailService = new Mock<IEmailService>();
        var templateService = new Mock<ITemplateService>();
        var jwtOptions = CreateJwtOptions();
        var frontendOptions = CreateFrontendOptions();

        var accountService = new AccountService(
            userManager.Object,
            jwtOptions,
            timeProvider,
            emailService.Object,
            templateService.Object,
            context,
            frontendOptions);

        var request = new RegisterRequest
        {
            Email = "existing@example.com",
            Name = "Test",
            Surname = "User",
            Role = "User"
        };

        // Act & Assert
        await Assert.ThrowsAsync<BadRequestException>(() =>
            accountService.RegisterAsync(request, "en"));
    }

    [Fact]
    public async Task RefreshTokenAsync_ShouldReturnNewTokens_WhenTokenIsValid()
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
            Email = "test@example.com",
            UserName = "test@example.com",
            Name = "Test",
            Surname = "Test"
        };
        context.Users.Add(user);

        var refreshToken = new RefreshToken
        {
            Token = "valid-refresh-token",
            UserId = userId,
            User = user,
            IsRevoked = false,
            ExpiresAt = timeProvider.GetUtcNow().AddDays(10)
        };
        context.RefreshTokens.Add(refreshToken);
        await context.SaveChangesAsync();

        var userManager = CreateMockUserManager();
        userManager.Setup(x => x.GetRolesAsync(It.IsAny<User>()))
            .ReturnsAsync(new List<string> { "User" });

        var emailService = new Mock<IEmailService>();
        var templateService = new Mock<ITemplateService>();
        var jwtOptions = CreateJwtOptions();
        var frontendOptions = CreateFrontendOptions();

        var accountService = new AccountService(
            userManager.Object,
            jwtOptions,
            timeProvider,
            emailService.Object,
            templateService.Object,
            context,
            frontendOptions);

        var request = new RefreshTokenRequest
        {
            RefreshToken = "valid-refresh-token"
        };

        // Act
        var result = await accountService.RefreshTokenAsync(request);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result.JwtToken);
        Assert.NotEmpty(result.RefreshToken);

        var oldToken = await context.RefreshTokens.FirstAsync(rt => rt.Token == "valid-refresh-token");
        Assert.True(oldToken.IsRevoked);

        var newToken = await context.RefreshTokens.FirstAsync(rt => rt.Token == result.RefreshToken);
        Assert.False(newToken.IsRevoked);
    }

    [Fact]
    public async Task RefreshTokenAsync_ShouldThrowUnauthorized_WhenTokenIsInvalid()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ShelterManagerContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var timeProvider = TimeProvider.System;
        await using var context = new ShelterManagerContext(options, timeProvider);

        var userManager = CreateMockUserManager();
        var emailService = new Mock<IEmailService>();
        var templateService = new Mock<ITemplateService>();
        var jwtOptions = CreateJwtOptions();
        var frontendOptions = CreateFrontendOptions();

        var accountService = new AccountService(
            userManager.Object,
            jwtOptions,
            timeProvider,
            emailService.Object,
            templateService.Object,
            context,
            frontendOptions);

        var request = new RefreshTokenRequest
        {
            RefreshToken = "invalid-token"
        };

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedException>(() =>
            accountService.RefreshTokenAsync(request));
    }

    [Fact]
    public async Task SendResetPasswordEmailAsync_ShouldSendEmail_WhenUserExists()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ShelterManagerContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var timeProvider = TimeProvider.System;
        await using var context = new ShelterManagerContext(options, timeProvider);

        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = "test@example.com",
            UserName = "test@example.com",
            Name = "Test",
            Surname = "User"
        };

        var userManager = CreateMockUserManager();
        userManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync(user);
        userManager.Setup(x => x.GeneratePasswordResetTokenAsync(It.IsAny<User>()))
            .ReturnsAsync("reset-token");

        var emailService = new Mock<IEmailService>();
        var templateService = new Mock<ITemplateService>();
        templateService.Setup(x => x.LoadTemplate(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()))
            .Returns("<html>Reset email</html>");

        var jwtOptions = CreateJwtOptions();
        var frontendOptions = CreateFrontendOptions();

        var accountService = new AccountService(
            userManager.Object,
            jwtOptions,
            timeProvider,
            emailService.Object,
            templateService.Object,
            context,
            frontendOptions);

        var request = new ForgotPasswordRequest
        {
            Email = "test@example.com"
        };

        // Act
        await accountService.SendResetPasswordEmailAsync(request, "en");

        // Assert
        emailService.Verify(x => x.SendEmailAsync(
            "test@example.com",
            "Test User",
            It.IsAny<string>(),
            It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task SendResetPasswordEmailAsync_ShouldThrowNotFound_WhenUserDoesNotExist()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ShelterManagerContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var timeProvider = TimeProvider.System;
        await using var context = new ShelterManagerContext(options, timeProvider);

        var userManager = CreateMockUserManager();
        userManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync((User?)null);

        var emailService = new Mock<IEmailService>();
        var templateService = new Mock<ITemplateService>();
        var jwtOptions = CreateJwtOptions();
        var frontendOptions = CreateFrontendOptions();

        var accountService = new AccountService(
            userManager.Object,
            jwtOptions,
            timeProvider,
            emailService.Object,
            templateService.Object,
            context,
            frontendOptions);

        var request = new ForgotPasswordRequest
        {
            Email = "nonexistent@example.com"
        };

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() =>
            accountService.SendResetPasswordEmailAsync(request, "en"));
    }

    [Fact]
    public async Task ResetPasswordAsync_ShouldResetPassword_WhenTokenIsValid()
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
            Email = "test@example.com",
            UserName = "test@example.com",
            Name = "Test",
            Surname = "Test"
        };
        context.Users.Add(user);

        var refreshToken = new RefreshToken
        {
            Token = "token-to-revoke",
            UserId = userId,
            IsRevoked = false,
            ExpiresAt = timeProvider.GetUtcNow().AddDays(10)
        };
        context.RefreshTokens.Add(refreshToken);
        await context.SaveChangesAsync();

        var userManager = CreateMockUserManager();
        userManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync(user);
        userManager.Setup(x => x.ResetPasswordAsync(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Success);

        var emailService = new Mock<IEmailService>();
        var templateService = new Mock<ITemplateService>();
        var jwtOptions = CreateJwtOptions();
        var frontendOptions = CreateFrontendOptions();

        var accountService = new AccountService(
            userManager.Object,
            jwtOptions,
            timeProvider,
            emailService.Object,
            templateService.Object,
            context,
            frontendOptions);

        var request = new ResetPasswordRequest
        {
            Email = "test@example.com",
            Token = "valid-reset-token",
            NewPassword = "NewPassword123!"
        };

        // Act
        await accountService.ResetPasswordAsync(request);

        // Assert
        userManager.Verify(x => x.ResetPasswordAsync(user, It.IsAny<string>(), "NewPassword123!"), Times.Once);

        var revokedToken = await context.RefreshTokens.FirstAsync();
        Assert.True(revokedToken.IsRevoked);
    }

    [Fact]
    public async Task ResetPasswordAsync_ShouldThrowNotFound_WhenUserDoesNotExist()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ShelterManagerContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var timeProvider = TimeProvider.System;
        await using var context = new ShelterManagerContext(options, timeProvider);

        var userManager = CreateMockUserManager();
        userManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync((User?)null);

        var emailService = new Mock<IEmailService>();
        var templateService = new Mock<ITemplateService>();
        var jwtOptions = CreateJwtOptions();
        var frontendOptions = CreateFrontendOptions();

        var accountService = new AccountService(
            userManager.Object,
            jwtOptions,
            timeProvider,
            emailService.Object,
            templateService.Object,
            context,
            frontendOptions);

        var request = new ResetPasswordRequest
        {
            Email = "nonexistent@example.com",
            Token = "some-token",
            NewPassword = "NewPassword123!"
        };

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() =>
            accountService.ResetPasswordAsync(request));
    }

    [Fact]
    public async Task ChangePasswordAsync_ShouldChangePassword_WhenCurrentPasswordIsValid()
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
            Email = "test@example.com",
            UserName = "test@example.com",
            MustChangePassword = true,
            Name = "Test",
            Surname = "Test"
        };
        context.Users.Add(user);

        var refreshToken = new RefreshToken
        {
            Token = "token-to-revoke",
            UserId = userId,
            IsRevoked = false,
            ExpiresAt = timeProvider.GetUtcNow().AddDays(10)
        };
        context.RefreshTokens.Add(refreshToken);
        await context.SaveChangesAsync();

        var userManager = CreateMockUserManager();
        userManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync(user);
        userManager.Setup(x => x.ChangePasswordAsync(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Success);
        userManager.Setup(x => x.UpdateAsync(It.IsAny<User>()))
            .ReturnsAsync(IdentityResult.Success)
            .Callback<User>(u => u.MustChangePassword = false);

        var emailService = new Mock<IEmailService>();
        var templateService = new Mock<ITemplateService>();
        var jwtOptions = CreateJwtOptions();
        var frontendOptions = CreateFrontendOptions();

        var accountService = new AccountService(
            userManager.Object,
            jwtOptions,
            timeProvider,
            emailService.Object,
            templateService.Object,
            context,
            frontendOptions);

        var request = new ChangePasswordRequest
        {
            Email = "test@example.com",
            CurrentPassword = "OldPassword123!",
            NewPassword = "NewPassword123!"
        };

        // Act
        await accountService.ChangePasswordAsync(request);

        // Assert
        userManager.Verify(x => x.ChangePasswordAsync(user, "OldPassword123!", "NewPassword123!"), Times.Once);
        userManager.Verify(x => x.UpdateAsync(It.IsAny<User>()), Times.Once);

        var revokedToken = await context.RefreshTokens.FirstAsync();
        Assert.True(revokedToken.IsRevoked);
    }

    [Fact]
    public async Task ChangePasswordAsync_ShouldThrowNotFound_WhenUserDoesNotExist()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ShelterManagerContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var timeProvider = TimeProvider.System;
        await using var context = new ShelterManagerContext(options, timeProvider);

        var userManager = CreateMockUserManager();
        userManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync((User?)null);

        var emailService = new Mock<IEmailService>();
        var templateService = new Mock<ITemplateService>();
        var jwtOptions = CreateJwtOptions();
        var frontendOptions = CreateFrontendOptions();

        var accountService = new AccountService(
            userManager.Object,
            jwtOptions,
            timeProvider,
            emailService.Object,
            templateService.Object,
            context,
            frontendOptions);

        var request = new ChangePasswordRequest
        {
            Email = "nonexistent@example.com",
            CurrentPassword = "OldPassword123!",
            NewPassword = "NewPassword123!"
        };

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() =>
            accountService.ChangePasswordAsync(request));
    }
}