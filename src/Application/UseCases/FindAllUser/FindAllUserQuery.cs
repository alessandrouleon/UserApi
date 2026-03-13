using MediatR;
using UserApication.DTOs;
using UserApin.Common;

namespace UserApication.UseCases.FindAllUser;

public sealed record FindAllUserQuery(
    int Page = 1,
    int PageSize = 10,
    string? Search = null
) : IRequest<Result<PagedResultDto<UserResponseDto>>>;
