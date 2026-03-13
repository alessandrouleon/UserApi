using AutoMapper;
using UserApication.DTOs;
using UserApin.Entities;

namespace UserApication.Mappings;

public sealed class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserResponseDto>()
            .ConstructUsing(u => new UserResponseDto(
                u.Id,
                u.Name,
                u.Email,
                u.IsActive,
                u.CreatedAt,
                u.UpdatedAt));
    }
}
