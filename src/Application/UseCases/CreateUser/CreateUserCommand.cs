using MediatR;
using UserApication.DTOs;
using UserApin.Common;

namespace UserApication.UseCases.CreateUser;

public sealed record CreateUserCommand(
    string Name,
    string Email,
    string Password
) : IRequest<Result<UserResponseDto>>;
