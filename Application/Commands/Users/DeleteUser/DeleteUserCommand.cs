using MediatR;

namespace Application.Commands.Users;

public sealed record DeleteUserCommand(
    int UserId
) : IRequest;