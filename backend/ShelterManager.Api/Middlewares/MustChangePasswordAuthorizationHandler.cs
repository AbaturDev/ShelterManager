using Microsoft.AspNetCore.Authorization;
using ShelterManager.Api.Utils;
using ShelterManager.Core.Services.Abstractions;

namespace ShelterManager.Api.Middlewares;

public class MustChangePasswordAuthorizationHandler : AuthorizationHandler<MustChangePasswordRequirement>
{
    private readonly IUserContextService _userContext;
    
    public MustChangePasswordAuthorizationHandler(IUserContextService userContext)
    {
        _userContext = userContext;
    }
    
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MustChangePasswordRequirement requirement)
    {
        var mustChangePassword = _userContext.GetMustChangePassword();

        if (mustChangePassword is not null && mustChangePassword == false)
        {
            context.Succeed(requirement);
        }
        
        return Task.CompletedTask;
    }
}