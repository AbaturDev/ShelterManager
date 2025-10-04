using System.Security.Claims;

namespace ShelterManager.Core.Services.Abstractions;

public interface IUserContextService
{
    Guid? GetCurrentUserId();
}