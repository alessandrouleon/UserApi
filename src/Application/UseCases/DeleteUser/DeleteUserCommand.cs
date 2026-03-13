using MediatR;
using UserApin.Common;

namespace UserApication.UseCases.DeleteUser;

public sealed record DeleteUserCommand(Guid Id) : IRequest<Result>;
