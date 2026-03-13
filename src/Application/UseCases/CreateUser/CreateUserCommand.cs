using MediatR;
using UserApication.DTOs;
using UserApin.Common;

namespace UserApication.UseCases.CreateUser;

/// <summary>CQRS Command — creates a new user and returns the persisted representation.</summary>
public sealed record CreateUserCommand(
    string Name,
    string Email,
    string Password
) : IRequest<Result<UserResponseDto>>;
