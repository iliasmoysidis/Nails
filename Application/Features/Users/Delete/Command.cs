using MediatR;

namespace Application.Features.Users.Delete;

public sealed record Command(
    int UserId
) : IRequest;