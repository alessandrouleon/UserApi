using MediatR;
using UserApication.DTOs;
using UserApin.Common;

namespace UserApication.UseCases.UpdateUser;

public sealed record UpdateUserCommand(
    Guid Id,
    string Name,
    string Email,
    string? Password = null
) : IRequest<Result<UserResponseDto>>;
