using MediatR;

namespace Application.Users.Delete;

public sealed record Command(
    int UserId
) : IRequest;