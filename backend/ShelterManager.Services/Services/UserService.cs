using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ShelterManager.Core.Exceptions;
using ShelterManager.Core.Services.Abstractions;
using ShelterManager.Database.Entities;
using ShelterManager.Services.Dtos.Commons;
using ShelterManager.Services.Dtos.Users;
using ShelterManager.Services.Extensions;
using ShelterManager.Services.Mappers;
using ShelterManager.Services.Services.Abstractions;

namespace ShelterManager.Services.Services;

public class UserService : IUserService
{
    private readonly UserManager<User> _userManager;
    private readonly IUserContextService _userContext;
    private readonly ILogger<UserService> _logger;
    
    public UserService(UserManager<User> userManager, IUserContextService userContext, ILogger<UserService> logger)
    {
        _userManager = userManager;
        _userContext = userContext;
        _logger = logger;
    }
    
    public async Task<PaginatedResponse<UserDto>> ListUsersAsync(PageQueryFilter queryFilter, CancellationToken ct)
    {
        var users = _userManager.Users
            .AsNoTracking()
            .AsQueryable();
        
        var totalCount = await users.CountAsync(ct);

        var items = await users
            .Select(x => UserMappers.MapToUserDto(x))
            .Paginate(queryFilter.Page, queryFilter.PageSize)
            .ToListAsync(ct);
        
        return new PaginatedResponse<UserDto>(items, queryFilter.Page, queryFilter.PageSize, totalCount);
    }

    public async Task<UserDetailsDto> GetUserByIdAsync(Guid id, CancellationToken ct)
    {
        var user = await _userManager.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, ct);
        
        if (user is null)
        {
            throw new NotFoundException("User not found");
        }
        
        var role = (await _userManager.GetRolesAsync(user)).First();
        
        var userDto = UserMappers.MapToUserDetailsDto(user, role);
        
        return userDto;
    }

    public async Task<UserDetailsDto> GetCurrentUserAsync(CancellationToken ct)
    {
        var userId = _userContext.GetCurrentUserId();

        var user = await _userManager.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == userId, ct);
        
        if (user is null)
        {
            _logger.LogWarning("User from current token doesn't exist in database");
            throw new NotFoundException("User not found");
        }
        
        var role = (await _userManager.GetRolesAsync(user)).First();
        
        var userDto = UserMappers.MapToUserDetailsDto(user, role);

        return userDto;
    }

    public async Task SoftDeleteUserAsync(Guid id, CancellationToken ct)
    {
        var user = await _userManager.Users
            .FirstOrDefaultAsync(x => x.Id == id, ct);
        
        if (user is null)
        {
            throw new NotFoundException("User not found");
        }
        
        user.IsDeleted = true;
        
        await _userManager.UpdateAsync(user);
    }
}