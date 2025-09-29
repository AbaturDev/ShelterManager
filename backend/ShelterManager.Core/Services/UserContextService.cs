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
    
    public ClaimsPrincipal? GetCurrentUser()
    {
        return _httpContext.HttpContext?.User;
    }
}