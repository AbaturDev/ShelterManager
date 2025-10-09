using ShelterManager.Database.Entities;
using ShelterManager.Services.Dtos.Users;

namespace ShelterManager.Services.Mappers;

public static class UserMappers
{
    public static UserDto MapToUserDto(User user)
    {
        return new UserDto
        {
            Id = user.Id,
            Email = user.Email!,
            Name = user.Name,
            Surname = user.Surname,
        };
    }
    
    public static UserDetailsDto MapToUserDetailsDto(User user, string role)
    {
        return new UserDetailsDto
        {
            Id = user.Id,
            Email = user.Email!,
            Name = user.Name,
            Surname = user.Surname,
            MustChangePassword = user.MustChangePassword,
            Role = role,
        };
    }
}