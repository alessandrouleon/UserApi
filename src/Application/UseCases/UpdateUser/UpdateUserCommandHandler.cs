using AutoMapper;
using MediatR;
using UserApication.DTOs;
using UserApication.Interfaces;
using UserApin.Common;
using UserApin.Interfaces.Repositories;
using UserApin.ValueObjects;

namespace UserApication.UseCases.UpdateUser;

public sealed class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Result<UserResponseDto>>
{
    private readonly IUserRepository _repository;
    private readonly IHashService _hashService;
    private readonly IMapper _mapper;

    public UpdateUserCommandHandler(
        IUserRepository repository,
        IHashService hashService,
        IMapper mapper)
    {
        _repository = repository;
        _hashService = hashService;
        _mapper = mapper;
    }

    public async Task<Result<UserResponseDto>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (user is null)
            return Result<UserResponseDto>.Failure("User not found.");

        if (!user.IsActive)
            return Result<UserResponseDto>.Failure("Cannot update an inactive user.");

        var emailTaken = await _repository.EmailExistsAsync(request.Email, excludeId: request.Id, cancellationToken: cancellationToken);
        if (emailTaken)
            return Result<UserResponseDto>.Failure("A user with this email already exists.");

        Password? newPassword = null;
        if (!string.IsNullOrWhiteSpace(request.Password))
        {
            var hash = _hashService.Hash(request.Password);
            newPassword = Password.FromHash(hash);
        }

        var updateResult = user.Update(request.Name, request.Email, newPassword);
        if (updateResult.IsFailure)
            return Result<UserResponseDto>.Failure(updateResult.Error!);

        _repository.Update(user);
        await _repository.SaveChangesAsync(cancellationToken);

        return Result<UserResponseDto>.Success(_mapper.Map<UserResponseDto>(user));
    }
}
