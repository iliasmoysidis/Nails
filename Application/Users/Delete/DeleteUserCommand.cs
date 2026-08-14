using MediatR;

namespace Application.Users.Delete;

public sealed record DeleteUserCommand(
    int UserId
) : IRequest;