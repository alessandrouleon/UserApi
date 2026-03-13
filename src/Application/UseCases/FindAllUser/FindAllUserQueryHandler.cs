using AutoMapper;
using MediatR;
using UserApication.DTOs;
using UserApin.Common;
using UserApin.Interfaces.Repositories;

namespace UserApication.UseCases.FindAllUser;

public sealed class FindAllUserQueryHandler : IRequestHandler<FindAllUserQuery, Result<PagedResultDto<UserResponseDto>>>
{
    private readonly IUserRepository _repository;
    private readonly IMapper _mapper;

    public FindAllUserQueryHandler(IUserRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<Result<PagedResultDto<UserResponseDto>>> Handle(FindAllUserQuery request, CancellationToken cancellationToken)
    {
        var page = request.Page < 1 ? 1 : request.Page;
        var pageSize = request.PageSize is < 1 or > 100 ? 10 : request.PageSize;

        var (items, totalCount) = await _repository.GetAllAsync(page, pageSize, request.Search, cancellationToken);

        var dtos = _mapper.Map<IEnumerable<UserResponseDto>>(items);
        var result = PagedResultDto<UserResponseDto>.Create(dtos, totalCount, page, pageSize);

        return Result<PagedResultDto<UserResponseDto>>.Success(result);
    }
}
