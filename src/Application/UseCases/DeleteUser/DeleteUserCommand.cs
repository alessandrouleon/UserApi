using MediatR;
using UserApin.Common;

namespace UserApication.UseCases.DeleteUser;

/// <summary>CQRS Command — soft-deletes a user by ID.</summary>
public sealed record DeleteUserCommand(Guid Id) : IRequest<Result>;
