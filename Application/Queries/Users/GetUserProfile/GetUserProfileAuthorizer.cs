using Application.Abstractions.Authorization;
using Application.Contexts;
using Application.Exceptions;

namespace Application.Queries.Users;

public sealed class GetUserProfileAuthorizer
    : IAuthorizer<GetUserProfileQuery>
{
    private readonly IRequestContext _context;

    public GetUserProfileAuthorizer(IRequestContext context)
    {
        _context = context;
    }

    public Task AuthorizeAsync(
        GetUserProfileQuery request,
        CancellationToken ct
    )
    {
        if (!_context.IsUser)
            throw new ApplicationLayerForbiddenException("User role required.");

        if (_context.ActorId != request.UserId)
            throw new ApplicationLayerForbiddenException("Cannot access another user's profile.");

        return Task.CompletedTask;
    }
}