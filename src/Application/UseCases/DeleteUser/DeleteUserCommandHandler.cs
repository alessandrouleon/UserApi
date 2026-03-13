using MediatR;
using UserApin.Common;
using UserApin.Interfaces.Repositories;

namespace UserApication.UseCases.DeleteUser;

public sealed class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, Result>
{
    private readonly IUserRepository _repository;

    public DeleteUserCommandHandler(IUserRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (user is null)
            return Result.Failure("User not found.");

        if (!user.IsActive)
            return Result.Failure("User is already inactive.");

        user.Delete();
        _repository.Update(user);
        await _repository.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
