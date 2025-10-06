namespace ShelterManager.Core.Services.Abstractions;

public interface IUserContextService
{
    Guid? GetCurrentUserId();
    bool? GetMustChangePassword();
}