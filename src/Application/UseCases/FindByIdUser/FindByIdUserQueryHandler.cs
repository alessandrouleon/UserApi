using AutoMapper;
using MediatR;
using UserApication.DTOs;
using UserApin.Common;
using UserApin.Interfaces.Repositories;

namespace UserApication.UseCases.FindByIdUser;

public sealed class FindByIdUserQueryHandler : IRequestHandler<FindByIdUserQuery, Result<UserResponseDto>>
{
    private readonly IUserRepository _repository;
    private readonly IMapper _mapper;

    public FindByIdUserQueryHandler(IUserRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<Result<UserResponseDto>> Handle(FindByIdUserQuery request, CancellationToken cancellationToken)
    {
        var user = await _repository.GetByIdAsync(request.Id, cancellationToken);

        if (user is null || !user.IsActive)
            return Result<UserResponseDto>.Failure("User not found.");

        return Result<UserResponseDto>.Success(_mapper.Map<UserResponseDto>(user));
    }
}
