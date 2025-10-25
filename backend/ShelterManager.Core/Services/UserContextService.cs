using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using ShelterManager.Core.Services.Abstractions;

namespace ShelterManager.Core.Services;

public class UserContextService : IUserContextService
{
    private readonly IHttpContextAccessor _httpContext;
    
    public UserContextService(IHttpContextAccessor httpContext)
    {
        _httpContext = httpContext;
    }

    public ClaimsPrincipal? User => _httpContext.HttpContext?.User;

    public Guid? GetCurrentUserId()
    {
        if (User is null)
        {
            return null;
        }

        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        return userId is not null ? Guid.Parse(userId) : null;
    }

    public bool? GetMustChangePassword()
    {
        if (User is null)
        {
            return null;
        }
        
        var mustChangePassword = _httpContext.HttpContext.User.FindFirst("mustChangePassword");

        if (mustChangePassword is not null && bool.TryParse(mustChangePassword.Value, out var mustChangePasswordResult))
        {
            return mustChangePasswordResult;
        }
        
        return null;
    }
}