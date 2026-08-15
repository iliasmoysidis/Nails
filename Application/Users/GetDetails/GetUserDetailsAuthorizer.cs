using Application.Common.Abstractions.Authorization;
using Application.Common.Contexts;
using Application.Common.Exceptions;

namespace Application.Users.GetDetails;

public sealed class GetUserDetailsAuthorizer
    : IAuthorizer<GetUserDetailsQuery>
{
    private readonly IRequestContext _context;

    public GetUserDetailsAuthorizer(IRequestContext context)
    {
        _context = context;
    }

    public Task AuthorizeAsync(
        GetUserDetailsQuery request,
        CancellationToken ct
    )
    {
        if (!_context.IsUser)
            throw new ApplicationLayerForbiddenException("User role required.");

        if (_context.ActorId != request.UserId)
            throw new ApplicationLayerForbiddenException("Cannot access another user's details.");

        return Task.CompletedTask;
    }
}
