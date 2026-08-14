using Application.Users.Common.Queries;
using Application.Common.Exceptions;

namespace Application.Users.GetProfile;

public sealed class GetUserProfileHandler
{
    private readonly IUserQueries _queries;

    public GetUserProfileHandler(IUserQueries queries)
    {
        _queries = queries;
    }

    public async Task<UserProfileDTO> Handle(GetUserProfileQuery query, CancellationToken ct)
    {
        var profile = await _queries.GetUserProfileAsync(query.UserId, ct)
            ?? throw new ApplicationLayerNotFoundException("User not found.");

        return profile;
    }
}