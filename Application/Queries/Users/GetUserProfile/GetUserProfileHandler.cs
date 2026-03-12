using Application.Abstractions.Queries;
using Application.DTO.User;
using Application.Exceptions;
using Application.Guards;

namespace Application.Queries.Users;

public sealed class GetUserProfileHandler
{
    private readonly AuthorizationGuard _auth;
    private readonly IUserQueries _queries;

    public GetUserProfileHandler(
        AuthorizationGuard auth,
        IUserQueries queries
    )
    {
        _auth = auth;
        _queries = queries;
    }

    public async Task<UserProfileDTO> Handle(GetUserProfileQuery query, CancellationToken ct)
    {
        _auth.EnsureUser();
        _auth.EnsureSelf(query.UserId);

        return await _queries.GetUserProfileAsync(query.UserId, ct)
            ?? throw new ApplicationLayerNotFoundException("User not found.");
    }
}