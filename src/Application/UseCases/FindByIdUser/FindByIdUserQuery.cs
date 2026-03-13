using MediatR;
using UserApication.DTOs;
using UserApin.Common;

namespace UserApication.UseCases.FindByIdUser;

/// <summary>CQRS Query — retrieves a single active user by ID.</summary>
public sealed record FindByIdUserQuery(Guid Id) : IRequest<Result<UserResponseDto>>;
