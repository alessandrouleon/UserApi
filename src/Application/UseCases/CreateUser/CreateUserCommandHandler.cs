using AutoMapper;
using MediatR;
using UserApication.DTOs;
using UserApication.Interfaces;
using UserApin.Common;
using UserApin.Entities;
using UserApin.Interfaces.Repositories;
using UserApin.ValueObjects;

namespace UserApication.UseCases.CreateUser;

public sealed class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Result<UserResponseDto>>
{
    private readonly IUserRepository _repository;
    private readonly IHashService _hashService;
    private readonly IMapper _mapper;

    public CreateUserCommandHandler(
        IUserRepository repository,
        IHashService hashService,
        IMapper mapper)
    {
        _repository = repository;
        _hashService = hashService;
        _mapper = mapper;
    }

    public async Task<Result<UserResponseDto>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var emailTaken = await _repository.EmailExistsAsync(request.Email, cancellationToken: cancellationToken);
        if (emailTaken)
            return Result<UserResponseDto>.Failure("A user with this email already exists.");

        var passwordHash = _hashService.Hash(request.Password);
        var password = Password.FromHash(passwordHash);

        var userResult = User.Create(request.Name, request.Email, password);
        if (userResult.IsFailure)
            return Result<UserResponseDto>.Failure(userResult.Error!);

        await _repository.AddAsync(userResult.Value!, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        return Result<UserResponseDto>.Success(_mapper.Map<UserResponseDto>(userResult.Value!));
    }
}
