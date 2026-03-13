using MediatR;
using UserApication.DTOs;
using UserApin.Common;

namespace UserApication.UseCases.FindByIdUser;

public sealed record FindByIdUserQuery(Guid Id) : IRequest<Result<UserResponseDto>>;
