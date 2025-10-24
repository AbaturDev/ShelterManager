using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ShelterManager.Core.Exceptions;
using ShelterManager.Core.Options;
using ShelterManager.Core.Services.Abstractions;
using ShelterManager.Core.Utils;
using ShelterManager.Database.Contexts;
using ShelterManager.Database.Entities;
using ShelterManager.Services.Constants;
using ShelterManager.Services.Dtos.Accounts;
using ShelterManager.Services.Services.Abstractions;

namespace ShelterManager.Services.Services;

public class AccountService : IAccountService
{
    private const int DefaultPasswordLength = 8;
    private const int RefreshTokenExpirationDays = 20;
    
    private readonly UserManager<User> _userManager;
    private readonly IOptions<JwtOptions> _jwtOptions;
    private readonly TimeProvider _timeProvider;
    private readonly IEmailService _emailService;
    private readonly ITemplateService _templateService;
    private readonly ShelterManagerContext _context;
    private readonly IOptions<FrontendOptions> _frontendOptions;
    
    public AccountService(UserManager<User> userManager, IOptions<JwtOptions> jwtOptions, TimeProvider timeProvider, IEmailService emailService, ITemplateService templateService, ShelterManagerContext context, IOptions<FrontendOptions> frontendOptions)
    {
        _userManager = userManager;
        _jwtOptions = jwtOptions;
        _timeProvider = timeProvider;
        _emailService = emailService;
        _templateService = templateService;
        _context = context;
        _frontendOptions = frontendOptions;
    }
    
    public async Task<LoginResponse> LoginAsync(LoginRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user is null || !await _userManager.CheckPasswordAsync(user, request.Password))
        {
            throw new BadRequestException("Invalid email or password");
        }

        var refreshToken = new RefreshToken
        {
            Token = GenerateRefreshToken(),   
            UserId = user.Id,
            IsRevoked = false,
            ExpiresAt = _timeProvider.GetUtcNow().AddDays(RefreshTokenExpirationDays),
        };
        
        _context.RefreshTokens.Add(refreshToken);
        await _context.SaveChangesAsync();
        
        var response = new LoginResponse
        {
            JwtToken = await GenerateToken(user),
            RefreshToken = refreshToken.Token,
        };
        
        return response;
    }

    public async Task RegisterAsync(RegisterRequest request, string languageCode)
    {
        var userExists = await _userManager.Users
            .AsNoTracking()
            .IgnoreQueryFilters()
            .AnyAsync(u => u.Email == request.Email);
        
        if (userExists)
        {
            throw new BadRequestException("Email is already taken");
        }
        
        var user = new User
        {
            Email = request.Email,
            UserName = request.Email,
            Name = request.Name,
            Surname = request.Surname,
            MustChangePassword = true,
        };
        
        var password = PasswordGenerator.GeneratePassword(DefaultPasswordLength);

        var result = await _userManager.CreateAsync(user, password);
        if (!result.Succeeded)
        {
            var errors = string.Join(";", result.Errors.Select(e => e.Description));
            
            throw new BadRequestException(errors);
        }
        
        var roleResult = await _userManager.AddToRoleAsync(user, request.Role);
        
        if (!roleResult.Succeeded)
        {
            var errors = string.Join(";", roleResult.Errors.Select(e => e.Description));
            
            throw new BadRequestException(errors);
        }
        
        var userFullName = $"{user.Name} {user.Surname}";

        var templateParameters = new Dictionary<string, string>()
        {
            { "User", userFullName },
            { "Password", password }
        };

        var path = Path.Combine("Templates", "Emails", languageCode, "RegisterEmail.html");
        var htmlMessage = _templateService.LoadTemplate(path, templateParameters);
        
        await _emailService.SendEmailAsync(user.Email, userFullName, RegisterEmailSubjects.GetSubject(languageCode), htmlMessage);
    }
    
    public async Task<LoginResponse> RefreshTokenAsync(RefreshTokenRequest request)
    {
        var refreshToken = await _context.RefreshTokens
            .Include(rt => rt.User)
            .FirstOrDefaultAsync(rt => rt.Token == request.RefreshToken);

        if (refreshToken is null || refreshToken.IsRevoked || refreshToken.ExpiresAt <= _timeProvider.GetUtcNow())
        {
            throw new UnauthorizedException("Invalid or expired refresh token");
        }
        
        refreshToken.IsRevoked = true;
        
        var newRefreshToken = new RefreshToken
        {
            IsRevoked = false,
            Token = GenerateRefreshToken(),
            UserId = refreshToken.User.Id,
            ExpiresAt = _timeProvider.GetUtcNow().AddDays(RefreshTokenExpirationDays),
        };
        
        _context.RefreshTokens.Add(newRefreshToken);
        await _context.SaveChangesAsync();
        
        return new LoginResponse
        {
            JwtToken = await GenerateToken(refreshToken.User),
            RefreshToken = newRefreshToken.Token
        };
    }

    public async Task SendResetPasswordEmailAsync(ForgotPasswordRequest request, string lang)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user is null)
        {
            throw new NotFoundException("User not found");
        }

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var encodedToken = WebUtility.UrlEncode(token);
        
        var userFullName = $"{user.Name} {user.Surname}";
        
        var templateParameters = new Dictionary<string, string>
        {
            { "User", userFullName },
            { "ResetLink", $"{_frontendOptions.Value.Url}/reset-password?token={encodedToken}&email={user.Email}" }
        };

        var path = Path.Combine("Templates", "Emails", lang, "ResetPasswordEmail.html");
        var htmlMessage = _templateService.LoadTemplate(path, templateParameters);
        await _emailService.SendEmailAsync(user.Email!, userFullName, ResetPasswordEmailSubjects.GetSubject(lang), htmlMessage);
    }

    public async Task ResetPasswordAsync(ResetPasswordRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user is null)
        {
            throw new NotFoundException("User not found");
        }

        var token = WebUtility.UrlDecode(request.Token);
        
        var result = await _userManager.ResetPasswordAsync(user, token, request.NewPassword);
        if (!result.Succeeded)
        {
            var errors = string.Join(";", result.Errors.Select(e => e.Description));
            throw new BadRequestException(errors);
        }
    }

    public async Task ChangePasswordAsync(ChangePasswordRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        
        if (user is null)
        {
            throw new NotFoundException("User not found");
        }
        
        var result = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);
        
        if (!result.Succeeded)
        {
            var errors = string.Join(";", result.Errors.Select(e => e.Description));
            
            throw new BadRequestException(errors);
        }
        
        user.MustChangePassword = false;
        await _userManager.UpdateAsync(user);
    }
    
    private static string GenerateRefreshToken()
    {
        var bytes = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(bytes);
        return Convert.ToBase64String(bytes);
    }
    
    private async Task<string> GenerateToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Value.Secret));

        var userRoles = await _userManager.GetRolesAsync(user);
        
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, $"{user.Email}"),
            new(ClaimTypes.Role, userRoles.First()),
            new(nameof(user.MustChangePassword), user.MustChangePassword.ToString())
        };
        
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = _timeProvider.GetUtcNow().UtcDateTime.AddMinutes(60),
            Issuer = _jwtOptions.Value.Issuer,
            Audience = _jwtOptions.Value.Audience,
            SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}