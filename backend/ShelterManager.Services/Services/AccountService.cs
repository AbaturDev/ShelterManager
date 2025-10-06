using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ShelterManager.Core.Exceptions;
using ShelterManager.Core.Options;
using ShelterManager.Database.Entities;
using ShelterManager.Services.Dtos.Accounts;
using ShelterManager.Services.Services.Abstractions;

namespace ShelterManager.Services.Services;

public class AccountService : IAccountService
{
    private readonly UserManager<User> _userManager;
    private readonly IOptions<JwtOptions> _jwtOptions;
    private readonly TimeProvider _timeProvider;
    
    public AccountService(UserManager<User> userManager, IOptions<JwtOptions> jwtOptions, TimeProvider timeProvider)
    {
        _userManager = userManager;
        _jwtOptions = jwtOptions;
        _timeProvider = timeProvider;
    }
    
    public async Task<LoginResponse> LoginAsync(LoginRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user is null || !await _userManager.CheckPasswordAsync(user, request.Password))
        {
            throw new BadRequestException("Invalid email or password");
        }

        var response = new LoginResponse
        {
            JwtToken = await GenerateToken(user)
        };
        
        return response;
    }

    public async Task RegisterAsync(RegisterRequest request)
    {
        if (await _userManager.FindByEmailAsync(request.Email) is not null)
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
        
        // TODO: generate random password and send new user email
        var password = "Haslo123!";

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
    
    private async Task<string> GenerateToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Value.Secret));

        var userRoles = await _userManager.GetRolesAsync(user);
        
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, $"{user.Email}"),
            new(nameof(user.MustChangePassword), user.MustChangePassword.ToString())
        };

        foreach (var role in userRoles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }
        
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